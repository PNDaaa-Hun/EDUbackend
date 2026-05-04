using System.ComponentModel.DataAnnotations;

namespace EDUBackEnd.Dtos.Auth
{
    public class Confirm2FADto
    {
        [Required]
        [StringLength(6, MinimumLength = 6)]
        public string Code { get; set; } = null!;
    }
}
