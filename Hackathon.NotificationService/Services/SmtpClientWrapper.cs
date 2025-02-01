using Hackathon.NotificationService.Interfaces;
using System.Net.Mail;

namespace Hackathon.NotificationService.Services
{
    public class SmtpClientWrapper : ISmtpClient
    {
        private readonly SmtpClient _smtpClient;

        public SmtpClientWrapper(string host, int port)
        {
            _smtpClient = new SmtpClient(host, port);
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
