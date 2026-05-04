using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

//Send help

namespace EDUBackEnd.Dtos.Adding
{
    public class AddStudentDto
    {
        public required string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public required string LastName { get; set; }
        [EmailAddress]
        public required string Email { get; set; }
        [Phone, MaxLength(12)]
        public string? PhoneNumber { get; set; }
        [ForeignKey("School")]
        public required int SchoolId { get; set; }

    }
}
