using Hackathon.Contract.Contracts;
using Hackathon.NotificationService.Interfaces;
using MassTransit;

namespace Hackathon.NotificationService.Consumers
{
    public class EmailNotificationConsumer : IConsumer<EmailNotificationMessage>
    {
        private readonly IEmailServices _emailService;

        public EmailNotificationConsumer(IEmailServices emailService) => _emailService = emailService;

        public async Task Consume(ConsumeContext<EmailNotificationMessage> context)
        {
            try
            {
                await _emailService.SendEmailAsync(context.Message.RecipientEmail, context.Message.Subject, context.Message.Body);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao enviar e-mail. Detalhes: {ex.Message}");

                throw;
            }
        }
    }
}