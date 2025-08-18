using Microshop.Contracts.Events;

using MassTransit;
using NotificationService.Consumers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<OrderCreatedConsumer>();

    x.UsingAzureServiceBus((context, cfg) =>
    {
        cfg.Host(builder.Configuration["AzureServiceBus:ConnectionString"]);

        cfg.SubscriptionEndpoint<OrderCreatedEvent>("notification-service", e =>
        {
            e.ConfigureConsumer<OrderCreatedConsumer>(context);
        });
    });
});

builder.Services.AddMassTransitHostedService();
builder.Services.AddControllers();

var app = builder.Build();
app.MapControllers();
app.Run();
