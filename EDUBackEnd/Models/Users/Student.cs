using EDUBackEnd.Enums;
using EDUBackEnd.Models.Timetable;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDUBackEnd.Models.Users
{
    public class Student
    {
        [Key, MaxLength(6)]
        public string Id { get; set; }
        public required string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public required string LastName { get; set; }
        public required int SchoolId { get; set; }
        public ICollection<Grade>? Grades { get; set; }
        [ForeignKey("Grade")]
        public int? GradeId { get; set; }
        public bool IsAbsence { get; set; }
        public int? SchoolClassId { get; set; }
        public int? AddressId { get; set; }
    }
}
