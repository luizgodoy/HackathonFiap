namespace Hackathon.NotificationService.Interfaces
{
    public interface IEmailServices
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}
