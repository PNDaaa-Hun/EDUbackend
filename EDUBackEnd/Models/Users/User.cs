using EDUBackEnd.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDUBackEnd.Models.Users
{
    public class User : IdentityUser
    {
        public Teacher? Teacher { get; set; }
        [ForeignKey("Teachers")]
        public int? TeacherId { get; set; }
        public Admin? Admin { get; set; }
        [ForeignKey("Admins")]
        public int? AdminId { get; set; }
        public Student? Student { get; set; }
        [ForeignKey("Students")]
        public string? StudentId { get; set; }
        public School? School { get; set; }
        [ForeignKey("Schools")]
        public int SchoolId { get; set; }

        public TwoFactorMethod TwoFactorMethod { get; set; }
        public string? ResetPasswordToken { get; set; }

        public DateTime? PasswordResetTokenExpiry { get; set; }
        public bool IsOnline { get; set; }
        public Address? Address { get; set; }
        [ForeignKey("Addresses")]
        public int? AddressId { get; set; }
        public int? ClassId { get; set; }
    }
}
