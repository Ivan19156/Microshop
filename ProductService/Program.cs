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

using ProductDbContext;
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
using Microsoft.Data.SqlClient; // для SqlException
using Microsoft.Extensions.Logging;

// The code now directly starts here, without the 'public class Program' and 'public static void Main' wrappers.
var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// ---------------- SERVICES ----------------

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

// MediatR
//builder.Services.AddMediatR(typeof(CreateProductCommand).Assembly);
// builder.Services.AddMediatR(cfg =>
//   cfg.RegisterServicesFromAssemblyContaining<Program>());
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<CreateProductHandler>());

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

// AutoMapper
//builder.Services.AddAutoMapper(typeof(CreateProductCommand).Assembly);

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

// using (var scope = app.Services.CreateScope())
//         {
//             var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//             const int maxRetries = 5;
//             int retryCount = 0;

//             while (true)
//             {
//                 try
//                 {
//                     dbContext.Database.Migrate();
//                     break;
//                 }
//                 catch (Microsoft.Data.SqlClient.SqlException)
//                 {
//                     if (++retryCount >= maxRetries) throw;
//                     Thread.Sleep(2000);
//                 }
//             }
//         }

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
