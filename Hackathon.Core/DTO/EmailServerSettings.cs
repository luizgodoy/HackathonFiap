namespace Hackathon.Core.DTO
{
    public class EmailServerSettings
    {
        public required string SmtpServer { get; set; }
        public required int Port { get; set; }
        public required bool EnableSsl { get; set; }
        public required bool UseDefaultCredentials { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string FromEmail { get; set; }
        public required string FromName { get; set; }
    }
}
