using System.ComponentModel.DataAnnotations;

namespace EDUBackEnd.Dtos.Update
{
    public class UpdateAdminDto
    {
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        [EmailAddress]
        public string? EmailAddress { get; set; }
        [Phone, MaxLength(12)]
        public string? PhoneNumber { get; set; }
        public required int SchoolId { get; set; }
    }
}
