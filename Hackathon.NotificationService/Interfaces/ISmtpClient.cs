using System.Net.Mail;

namespace Hackathon.NotificationService.Interfaces
{
    public interface ISmtpClient : IDisposable
    {
        Task SendMailAsync(MailMessage message);
    }

}
