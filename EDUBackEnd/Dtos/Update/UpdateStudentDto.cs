using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EDUBackEnd.Dtos.Update
{
    public class UpdateStudentDto
    {
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public required int SchoolId { get; set; }
        public int? GradeId { get; set; }
        public bool IsAbsence { get; set; }
        public int? SchoolClassId { get; set; }
    }
}
