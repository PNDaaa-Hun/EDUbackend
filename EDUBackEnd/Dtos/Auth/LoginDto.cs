using System.ComponentModel.DataAnnotations;

namespace EDUBackEnd.Dtos.Auth
{
    public class LoginDto
    {
        [EmailAddress]
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
