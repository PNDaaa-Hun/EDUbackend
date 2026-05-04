using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EDUBackEnd.Dtos.Update
{
    public class UpdateTeacherDto
    {
        public int Id { get; set; }

        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public required int SchoolId { get; set; }
    }
}
