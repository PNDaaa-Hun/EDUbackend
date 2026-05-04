using EDUBackEnd.Models.Email;
using EDUBackEnd.Services.Email;
using MailKit.Net.Smtp;
using MimeKit;
using Moq;
using Xunit;

// NEED FIX

public class EmailServiceTests
{
    [Fact]
    public async Task SendEmailAsync_ShouldSendEmail()
    {
        var smtpMock = new Mock<ISmtpClient>();
        
        smtpMock.SetupSequence(x => x.IsConnected)
            .Returns(false) // First call: not connected
            .Returns(true); // Second call: connected

        smtpMock.SetupSequence(x => x.IsAuthenticated)
            .Returns(false)
            .Returns(true);

        var config = new EmailConfiguration
        {
            SenderName = "Test",
            SenderEmail = "test@test.com",
            SmtpServer = "smtp.test.com",
            Port = 587,
            UseSSL = true,
            Username = "user",
            Password = "pass"
        };

        var service = new EmailService(config, smtpMock.Object);

        await service.SendEmailAsync("user@test.com", "Test", "<h1>Hello</h1>");

        smtpMock.Verify(x => x.ConnectAsync(
            It.IsAny<string>(),
            It.IsAny<int>(),
            It.IsAny<bool>(),
            It.IsAny<CancellationToken>()), Times.Once);

        smtpMock.Verify(x => x.AuthenticateAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()), Times.Once);

        smtpMock.Verify(x => x.SendAsync(
            It.IsAny<MimeMessage>(),
            It.IsAny<CancellationToken>(),
            null), Times.Once);

        smtpMock.Verify(x => x.DisconnectAsync(
            true,
            It.IsAny<CancellationToken>()), Times.Once);
    }
}