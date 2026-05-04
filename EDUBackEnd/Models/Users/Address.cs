using System.ComponentModel.DataAnnotations;

namespace EDUBackEnd.Models.Users
{
    public class Address
    {
        [Key]
        public int Id { get; set; }
        public required string Street { get; set; }
        public required string City { get; set; }
        public required string State { get; set; }
        public required string ZipCode { get; set; }
        public required string HouseNumber { get; set; }
    }
}
