using MimeKit;

namespace EDUBackEnd.Interfaces.Email
{
    public interface ISmtpClient
    {
        Task ConnectAsync(string host, int port, bool useSsl);
        Task AuthenticateAsync(string username, string password);
        Task SendAsync(MimeMessage message);
        Task DisconnectAsync(bool quit);
    }
}
