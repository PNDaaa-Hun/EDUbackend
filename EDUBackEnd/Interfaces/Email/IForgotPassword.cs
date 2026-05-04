namespace EDUBackEnd.Interfaces.Email
{
    public interface IForgotPassword
    {
        Task SendPasswordResetEmail(string email, string code, string username);
        Task SendEmailAsync(string to, string subject, string body);
    }
}
