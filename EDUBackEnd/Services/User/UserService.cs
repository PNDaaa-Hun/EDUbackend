using EDUBackEnd.Data;
using EDUBackEnd.Interfaces.User;
using EDUBackEnd.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace EDUBackEnd.Services.User
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAddressAsync(Address address, string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if(user == null)
            {
                throw new ArgumentException("User not found");
            }
            _context.Addresses.Add(address);
            user.Address = address;
            await _context.SaveChangesAsync();
        }

        public async Task<Address> GetAddressByIdAsync(int addressId)
        {
            Address? address = await _context.Addresses.FirstOrDefaultAsync(a => a.Id == addressId);
            if (address == null)
            {
                throw new ArgumentException("Address not found");
            }
            return address;
        }

        public async Task<List<Address>> GetAddresses()
        {
            return await _context.Addresses.AsNoTracking().ToListAsync();
        }

        public async Task UpdateAddressAsync(Address address)
        {
            if(address is null)
                throw new Exception("Address cannot be null");
            _context.Addresses.Update(address);
            await _context.SaveChangesAsync();
        }
    }
}
