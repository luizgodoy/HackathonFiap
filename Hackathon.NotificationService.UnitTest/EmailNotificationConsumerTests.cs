using Hackathon.Contract.Contracts;
using Hackathon.NotificationService.Consumers;
using Hackathon.NotificationService.Interfaces;
using MassTransit;
using Moq;
using Xunit;

namespace Hackathon.NotificationService.UnitTest
{
    public class EmailNotificationConsumerTests
    {
        private Mock<IEmailServices>? _emailServiceMock;
        private EmailNotificationConsumer? _consumer;

        [SetUp]
        public void Setup()
        {
            _emailServiceMock = new Mock<IEmailServices>();
            _consumer = new EmailNotificationConsumer(_emailServiceMock.Object);
        }

        [Test]
        public void Consume_ShouldCallSendEmailAsync_WithCorrectParameters()
        {
            // Arrange
            var message = new EmailNotificationMessage
            {
                RecipientEmail = "test@example.com",
                Subject = "Test Subject",
                Body = "Test Body"
            };

            var consumeContextMock = new Mock<ConsumeContext<EmailNotificationMessage>>();
            consumeContextMock.Setup(c => c.Message).Returns(message);

            // Act
            _consumer?.Consume(consumeContextMock.Object);

            // Assert
            _emailServiceMock?.Verify(s => s.SendEmailAsync(message.RecipientEmail, message.Subject, message.Body), Times.Once);
        }

        [Test]
        public Task Consume_ShouldThrowException_WhenEmailServiceFails()
        {
            // Arrange
            var message = new EmailNotificationMessage
            {
                RecipientEmail = "fail@example.com",
                Subject = "Fail Subject",
                Body = "Fail Body"
            };

            var consumeContextMock = new Mock<ConsumeContext<EmailNotificationMessage>>();
            consumeContextMock.Setup(c => c.Message).Returns(message);

            _emailServiceMock?
                .Setup(s => s.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("SMTP error"));

            // Act & Assert
            var exception = Assert.ThrowsAsync<Exception>(() => _consumer?.Consume(consumeContextMock.Object) ?? throw new Exception());

            Assert.That(exception?.Message, Is.EqualTo("SMTP error"));

            return Task.CompletedTask;
        }
    }
}