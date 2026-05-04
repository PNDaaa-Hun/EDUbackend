using System.ComponentModel.DataAnnotations;

//Send help
namespace EDUBackEnd.Dtos.Reset
{
    public class ForgotPassword
    {
        [EmailAddress]
        public required string Email { get; set; }

    }
}
