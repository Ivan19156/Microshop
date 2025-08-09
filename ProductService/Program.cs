// //
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

using MediatR;
using FluentValidation;
using FluentValidation.AspNetCore;
using Middleware;
using Cache;
using Entities;
using Application.Behaviors;
using Application.Products.Queries;
using Application.Products.Commands;
using Application.Products.Dtos;
using WebAPI.Controllers;
using Application.Products.Validators;

using Polly;
using Microsoft.Data.SqlClient; 
using Microsoft.Extensions.Logging;
using ProductService.Infrastructure.Persistence;
using ProductService.Infrastructure.Repository;
using ProductService.Application.Products.Mapping;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// ---------------- SERVICES ----------------

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

// MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateProductCommand).Assembly));


// FluentValidation
builder.Services.AddFluentValidationAutoValidation();

// Pipeline Behaviors
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));

// Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var redisConnection = configuration.GetConnectionString("Redis");
    return ConnectionMultiplexer.Connect(redisConnection!);
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = configuration.GetConnectionString("Redis");
});

builder.Services.AddScoped<ICacheService, RedisCacheService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// AutoMapper
builder.Services.AddAutoMapper((cfg => cfg.AddProfile<ProductProfile>()));

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Product API",
        Version = "v1"
    });
});

builder.Services.AddHealthChecks();
builder.Logging.AddConsole();
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!))
    };
});



var app = builder.Build();

Console.WriteLine($"Command Assembly: {typeof(Application.Products.Commands.CreateProductCommand).Assembly.FullName}");
Console.WriteLine($"Handler Assembly: {typeof(Application.Products.Commands.CreateProductHandler).Assembly.FullName}"); // Замініть на ваш хендлер

// ---------------- MIDDLEWARE ----------------

app.MapHealthChecks("/health");


    app.UseSwagger();
    app.UseSwaggerUI();


//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    const int maxRetries = 5;
    int retryCount = 0;

    while (true)
    {
        try
        {
            await dbContext.Database.MigrateAsync();
            Console.WriteLine("Database migration applied.");
            break;
        }
        catch (Microsoft.Data.SqlClient.SqlException ex)
        {
            retryCount++;
            Console.WriteLine($"Migration attempt {retryCount} failed: {ex.Message}");
            if (retryCount >= maxRetries) throw;
            await Task.Delay(2000);
        }
    }
}


app.Run();
