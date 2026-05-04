using EDUBackEnd.Interfaces.Email;
using EDUBackEnd.Models.Email;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using NETCore.MailKit.Core;

//Send help
namespace EDUBackEnd.Services.Email
{
    public class EmailService : IForgotPassword
    {
        private readonly EmailConfiguration _config;
        private readonly MailKit.Net.Smtp.ISmtpClient _smtpClient; 
        public EmailService(EmailConfiguration config
            , MailKit.Net.Smtp.ISmtpClient smtpClient)
        {
            _config = config;
            _smtpClient = smtpClient;

        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_config.SenderName, _config.SenderEmail));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;

            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = body
            };

            try
            {
                if (!_smtpClient.IsConnected)
                    await _smtpClient.ConnectAsync(_config.SmtpServer, _config.Port, _config.UseSSL);

                if (!_smtpClient.IsAuthenticated)
                    await _smtpClient.AuthenticateAsync(_config.Username, _config.Password);

                await _smtpClient.SendAsync(email);
            }
            finally
            {
                await _smtpClient.DisconnectAsync(true);
            }
        }
        public async Task SendPasswordResetEmail(string to, string resetLink, string username)
        {
            string subject = "Reset password";
            string body = $"<h2>Reset Password</h2>" +
                          $"<p><strong>Dear {username}</strong></p>" +
                          $"<p>If you have requested the reset password please click on the link:</p>" +
                          $"<a href=\"{resetLink}\">Reset password</a>";

            await SendEmailAsync(to, subject, body);
        }
    }
}
