namespace EDUBackEnd.Dtos.Adding
{
    public class AddAddressDto
    {
        public required string Street { get; set; }
        public required string City { get; set; }
        public required string State { get; set; }
        public required string ZipCode { get; set; }
        public required string HouseNumber { get; set; }
    }
}
