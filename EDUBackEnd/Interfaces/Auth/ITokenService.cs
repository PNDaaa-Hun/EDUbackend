namespace EDUBackEnd.Interfaces.Auth
{
    public interface ITokenService
    {
        Task<string> CreateToken(Models.Users.User user);
    }
}
