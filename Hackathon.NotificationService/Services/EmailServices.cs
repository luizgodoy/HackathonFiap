using Hackathon.Core.DTO;
using Hackathon.NotificationService.Interfaces;
using System.Net;
using System.Net.Mail;

namespace Hackathon.NotificationService.Services
{
    public class EmailServices : IEmailServices
    {
        private readonly EmailServerSettings _emailSettings;
        private readonly Func<ISmtpClient> _smtpClientFactory;

        public EmailServices(EmailServerSettings emailSettings, Func<ISmtpClient> smtpClientFactory)
        {
            _emailSettings = emailSettings ?? throw new ArgumentNullException(nameof(emailSettings));
            _smtpClientFactory = smtpClientFactory ?? throw new ArgumentNullException(nameof(smtpClientFactory));
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                if (_emailSettings == null)
                {
                    Console.WriteLine("Configurações do servidor de e-mail não realizadas");

                    return;
                }

                using (var client = _smtpClientFactory())
                {
                    var message = new MailMessage
                    {
                        From = new MailAddress(_emailSettings.FromEmail, _emailSettings.FromName),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true
                    };
                    message.To.Add(toEmail);

                    await client.SendMailAsync(message);

                    Console.WriteLine("E-mail enviado com sucesso!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao enviar e-mail: {ex.Message}");
                throw;
            }
        }
    }
}