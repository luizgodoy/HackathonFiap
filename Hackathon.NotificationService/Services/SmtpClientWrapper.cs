using Hackathon.Core.DTO;
using Hackathon.NotificationService.Interfaces;
using RabbitMQ.Client;
using System.Net;
using System.Net.Mail;

namespace Hackathon.NotificationService.Services
{
    public class SmtpClientWrapper : ISmtpClient
    {
        private readonly SmtpClient _smtpClient;

        public SmtpClientWrapper(EmailServerSettings settings)
        {
            _smtpClient = new SmtpClient(settings.SmtpServer, settings.Port)
            {
                Credentials = new NetworkCredential(settings.Username, settings.Password), // Adiciona autenticação
                EnableSsl = true, // Define se deve usar SSL
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false // Garante que credenciais personalizadas sejam usadas
            };
        }

        public async Task SendMailAsync(MailMessage message)
        {
            await _smtpClient.SendMailAsync(message);
        }

        public void Dispose()
        {
            _smtpClient.Dispose();
        }
    }
}
