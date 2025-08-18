using Azure.Messaging.ServiceBus;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OrderService.Application.Mapping;
using OrderService.Infrastructure.Data;
using OrderService.Infrastructure.Repositories.Interfaces;
using OrderService.Infrastructure.Repositories.Realizations; // Ensure MediatR namespace is included  
using System.Text;


var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));


builder.Services.AddDbContext<OrderDbContext>(options =>
   options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IOrderRepository, OrderRepository>();

  
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<OrderProfile>());

builder.Services.AddSingleton(_ =>
   new ServiceBusClient(builder.Configuration["AzureServiceBus:ConnectionString"]));

builder.Services.AddSingleton<ServiceBusSender>(provider =>
{
    var client = provider.GetRequiredService<ServiceBusClient>();
    return client.CreateSender("order-created");
});
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
builder.Services.AddMassTransit(x =>
{
    x.UsingAzureServiceBus((context, cfg) =>
    {
        cfg.Host(builder.Configuration["AzureServiceBus:ConnectionString"]);
    });
});



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
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
