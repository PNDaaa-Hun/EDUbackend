using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace EDUBackEnd.Dtos.Adding
{
    public class AddAdminDto
    {
        public required string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public required string LastName { get; set; }
        [ForeignKey("School")]
        public required int SchoolId { get; set; }
    }
}
