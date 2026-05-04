using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EDUBackEnd.Dtos.Adding
{
    public class AddTeacherDto
    {
        public required string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public required string LastName { get; set; }

        [ForeignKey("School")]
        public required int SchoolId { get; set; }

        public required List<int> AvailableTimeSlotIds { get; set; }
    }
}
