using System.ComponentModel.DataAnnotations;

namespace EDUBackEnd.Dtos.Auth
{
    public class TwoFactorDto
    {
        [Required]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(6, MinimumLength = 6)]
        public string Code { get; set; } = null!;

    }
}
