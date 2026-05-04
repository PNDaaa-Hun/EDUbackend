using EDUBackEnd.Enums;
using EDUBackEnd.Interfaces.Timetable;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDUBackEnd.Models.Users
{
    public class Teacher : ISchoolEntity
    {
        [Key]
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public required string LastName { get; set; }
        public required int SchoolId { get; set; }
        public Address? Address { get; set; }
        [ForeignKey("Addresses")]
        public int? AddressId { get; set; }

    }
}
