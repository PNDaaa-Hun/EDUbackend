using EDUBackEnd.Models.Users;

namespace EDUBackEnd.Interfaces.User
{
    public interface IUserService
    {
        Task AddAddressAsync(Address address, string userId);
        Task UpdateAddressAsync(Address address);
        Task<List<Address>> GetAddresses();
        Task<Address> GetAddressByIdAsync(int addressId);
    }
}
