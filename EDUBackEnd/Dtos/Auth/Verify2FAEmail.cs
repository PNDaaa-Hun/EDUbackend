namespace EDUBackEnd.Dtos.Auth
{
    public class Verify2FAEmail
    {
        public string Email { get; set; } = null!;
        public string Code { get; set; } = null!;
    }
}
