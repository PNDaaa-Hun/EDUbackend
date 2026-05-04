namespace EDUBackEnd.Dtos.Adding
{
    public class AddSchoolDto
    {
        public required string Name { get; set; }
        public required string Country { get; set; }
        public required string Region { get; set; }
        public required string PostalCode { get; set; }
        public required string City { get; set; }
        public required string Address { get; set; }
        public required int ActiveStudents { get; set; }
    }
}
