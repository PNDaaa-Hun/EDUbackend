using EDUBackEnd.Enums;

namespace EDUBackEnd.Dtos.Auth
{
    public class LoggerUserDto
    {
        public required string UserName { get; set; }
        public required string Role { get; set; }
        public int? SchoolClassId { get; set; }
        public required string Email { get; set; }
        public required string Token { get; set; }
    }
}
