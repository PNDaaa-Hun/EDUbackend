using System.ComponentModel.DataAnnotations;

namespace EDUBackEnd.Dtos.Auth
{
    public class LoginDtoStudent
    {
        public required string Id { get; set; }
        public required string Password { get; set; }
    }
}
