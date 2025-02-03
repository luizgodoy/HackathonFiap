using Hackathon.Core.DTO;
using Hackathon.NotificationService.Interfaces;
using Hackathon.NotificationService.Services;
using Moq;
using System.Net.Mail;

namespace Hackathon.NotificationService.UnitTest
{
    [TestFixture]
    public class EmailServicesTests
    {
        private Mock<ISmtpClient> _smtpClientMock;
        private EmailServices _emailService;
        private EmailServerSettings _emailSettings;

        [SetUp]
        public void Setup()
        {
            _emailSettings = new EmailServerSettings
            {
                SmtpServer = "smtp.example.com",
                Port = 587,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Username = "user@example.com",
                Password = "password",
                FromEmail = "noreply@example.com",
                FromName = "Test Sender"
            };

            _smtpClientMock = new Mock<ISmtpClient>();

            _emailService = new EmailServices(_emailSettings, _smtpClientMock.Object);
        }

        [Test]
        public async Task SendEmailAsync_ShouldSendEmail_WhenSettingsAreValid()
        {
            // Arrange
            _smtpClientMock.Setup(s => s.SendMailAsync(It.IsAny<MailMessage>())).Returns(Task.CompletedTask);

            // Act
            await _emailService.SendEmailAsync("receiver@example.com", "Test Subject", "Test Body");

            // Assert
            _smtpClientMock.Verify(s => s.SendMailAsync(It.Is<MailMessage>(m =>
                m.To[0].Address == "receiver@example.com" &&
                m.Subject == "Test Subject" &&
                m.Body == "Test Body"
            )), Times.Once);
        }

        [Test]
        public Task SendEmailAsync_ShouldThrowException_WhenSendFails()
        {
            // Arrange
            _smtpClientMock.Setup(s => s.SendMailAsync(It.IsAny<MailMessage>()))
                .ThrowsAsync(new SmtpException("SMTP error"));

            // Act & Assert
            var exception = Assert.ThrowsAsync<SmtpException>(
                async () => await _emailService.SendEmailAsync("receiver@example.com", "Test Subject", "Test Body")
            );

            Assert.That(exception?.Message, Is.EqualTo("SMTP error"));

            return Task.CompletedTask;
        }

    }
}
