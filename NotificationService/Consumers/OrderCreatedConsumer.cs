namespace NotificationService.Consumers;
//

using Microshop.Contracts.Events;
using MassTransit;
using SendGrid;
using SendGrid.Helpers.Mail;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

public class OrderCreatedConsumer : IConsumer<OrderCreatedEvent>
{
    private readonly ILogger<OrderCreatedConsumer> _logger;
    private readonly IConfiguration _config;

    public OrderCreatedConsumer(ILogger<OrderCreatedConsumer> logger, IConfiguration config)
    {
        _logger = logger;
        _config = config;
    }

    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        var message = context.Message;

        _logger.LogInformation("📩 Отримано замовлення {OrderId} для користувача {UserId}", message.OrderId, message.UserId);

        try
        {
            // Відправка Email
            await SendEmailAsync(message.CustomerEmail, message.OrderId);

            // Відправка SMS
            await SendSmsAsync(message.CustomerPhone, message.OrderId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Помилка при відправленні повідомлення");
        }
    }

    private async Task SendEmailAsync(string email, Guid orderId)
    {
        var apiKey = _config["SendGrid:ApiKey"];
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress("ilkiv2007ivan@gmail.com", "Your Shop");
        var subject = $"Підтвердження замовлення #{orderId}";
        var to = new EmailAddress(email);
        var plainTextContent = $"Ваше замовлення #{orderId} успішно створене!";
        var htmlContent = $"<strong>Ваше замовлення #{orderId} успішно створене!</strong>";
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        await client.SendEmailAsync(msg);
    }

    private async Task SendSmsAsync(string phone, Guid orderId)
    {
        TwilioClient.Init(_config["Twilio:AccountSid"], _config["Twilio:AuthToken"]);
        await MessageResource.CreateAsync(
            body: $"Ваше замовлення #{orderId} успішно створене!",
            from: new Twilio.Types.PhoneNumber(_config["Twilio:FromPhone"]),
            to: new Twilio.Types.PhoneNumber(phone)
        );
    }
}
