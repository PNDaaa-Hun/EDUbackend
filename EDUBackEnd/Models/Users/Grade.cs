using EDUBackEnd.Enums;
using EDUBackEnd.Models.Timetable;
using System.ComponentModel.DataAnnotations;

namespace EDUBackEnd.Models.Users
{
    public class Grade
    {
        [Key]
        public int Id { get; set; }
        public string? StudentId { get; set; }
        public Student? Student { get; set; }
        public GradeType GradeType { get; set; }
        public int? SubjectId { get; set; }
        public Subject? Subject { get; set; }
        [MaxLength(5),MinLength(1)]
        public int? Value { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; } 
    }
}
