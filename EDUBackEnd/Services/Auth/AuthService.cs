using EDUBackEnd.Data;
using EDUBackEnd.Services.Email;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using EDUBackEnd.Models;
using EDUBackEnd.Interfaces.Email;

namespace EDUBackEnd.Services.Auth
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly IForgotPassword _emailService;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context,
            IForgotPassword emailService,
            IConfiguration configuration)
        {
            _context = context;
            _emailService = emailService;
            _configuration = configuration;
        }


        public async Task<bool> ResetPasswordAsync(string token, string newPassword, string confirmNewPassword)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.ResetPasswordToken == token);

            if (user == null) return false;
            if (user.PasswordResetTokenExpiry < DateTime.UtcNow) return false;
            if (newPassword != confirmNewPassword) return false;

            user.ResetPasswordToken = null;
            user.PasswordResetTokenExpiry = null;

            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<bool> SendPasswordResetEmail(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null) return false;

            user.ResetPasswordToken = Guid.NewGuid().ToString();
            user.PasswordResetTokenExpiry = DateTime.UtcNow.AddMinutes(15);

            await _context.SaveChangesAsync();
            string resetLink;

            resetLink = _configuration["ResetLink"] + user.ResetPasswordToken;

            var username = user.StudentId ?? user.UserName;

            await _emailService.SendPasswordResetEmail(user.Email!, resetLink, username!);

            return true;
        }
    }
}
