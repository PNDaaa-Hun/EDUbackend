using System.ComponentModel.DataAnnotations;

namespace EDUBackEnd.Models.Users
{
    public class School
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Country { get; set; }    
        public required string Region { get; set; }
        public required string PostalCode { get; set; }
        public required string City { get; set; }
        public required string Address { get; set; }
    }
}
