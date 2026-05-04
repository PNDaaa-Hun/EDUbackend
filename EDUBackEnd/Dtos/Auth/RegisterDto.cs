namespace EDUBackEnd.Dtos.Auth
{
    public class RegisterDto
    {
        public required string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Phone { get; set; }
        public required int SchoolId { get; set; }
        public int? ClassId { get; set; }
    }
}
