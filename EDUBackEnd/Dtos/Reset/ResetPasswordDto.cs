using System.ComponentModel.DataAnnotations;

//Send help

namespace EDUBackEnd.Dtos.Reset
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "Password is required")]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "The password and confirmation do not match.")]
        public string NewConfirmPassword { get; set; }

        public required string Email { get; set; }
        public string? Token { get; set; }
    }
}
