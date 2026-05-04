using EDUBackEnd.Data;
using EDUBackEnd.Interfaces.Timetable.School;
using EDUBackEnd.Models.Users;
using EDUBackEnd.Services.User;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace EDUBackEnd.Tests.Services
{
    public class UserServiceTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            // Set up an isolated In-Memory Database for each test instance
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var mockSchoolService = new Mock<ICurrentSchoolService>();

            _context = new AppDbContext(mockSchoolService.Object,options);
            _userService = new UserService(_context);
        }

        // Clean up the in-memory database after each test runs
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task AddAddressAsync_ValidUser_AddsAddressAndAssignsToUser()
        {
            // Arrange
            var userId = "user-123";
            var user = new User { Id = userId , ClassId=1}; // Adjust based on your actual User model
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var newAddress = new Address { Id = 1, Street = "Original Street", City = "Original City", State = "Original State", ZipCode = "12345", HouseNumber = "1A" };

            // Act
            await _userService.AddAddressAsync(newAddress, userId);

            // Assert
            var savedUser = await _context.Users.FindAsync(userId);
            var savedAddress = await _context.Addresses.FindAsync(1);

            Assert.NotNull(savedAddress);
            Assert.NotNull(savedUser?.Address);
            Assert.Equal(newAddress.Id, savedUser.Address.Id);
        }

        [Fact]
        public async Task AddAddressAsync_UserNotFound_ThrowsArgumentException()
        {
            // Arrange
            var newAddress = new Address { Id = 1, Street = "Original Street", City = "Original City", State = "Original State", ZipCode = "12345", HouseNumber = "1A" };
            var nonExistentUserId = "user-404";

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
                _userService.AddAddressAsync(newAddress, nonExistentUserId));

            Assert.Equal("User not found", exception.Message);
        }

        [Fact]
        public async Task GetAddressByIdAsync_AddressExists_ReturnsAddress()
        {
            // Arrange
            var expectedAddress = new Address { Id = 1, Street = "Original Street", City = "Original City", State = "Original State", ZipCode = "12345", HouseNumber = "1A" };
            _context.Addresses.Add(expectedAddress);
            await _context.SaveChangesAsync();

            // Act
            var result = await _userService.GetAddressByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedAddress.Id, result.Id);
            Assert.Equal(expectedAddress.Street, result.Street);
        }

        [Fact]
        public async Task GetAddressByIdAsync_AddressNotFound_ThrowsArgumentException()
        {
            // Arrange
            int nonExistentAddressId = 99;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
                _userService.GetAddressByIdAsync(nonExistentAddressId));

            Assert.Equal("Address not found", exception.Message);
        }

        [Fact]
        public async Task GetAddresses_ReturnsAllAddresses()
        {
            // Arrange
            var addresses = new List<Address>
            {
                new Address {  Id = 1, Street = "Original Street", City = "Original City", State = "Original State", ZipCode = "12345", HouseNumber = "1A"},
                new Address { Id = 2, Street = "Test Street", City = "Test City", State = "Test State", ZipCode = "12355", HouseNumber = "2A"}
            };

            _context.Addresses.AddRange(addresses);
            await _context.SaveChangesAsync();

            // Act
            var result = await _userService.GetAddresses();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task UpdateAddressAsync_ValidAddress_UpdatesSuccessfully()
        {
            // Arrange
            var address = new Address { Id = 1, Street = "Original Street", City = "Original City", State = "Original State", ZipCode = "12345", HouseNumber = "1A"};
            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();

            // Detach the entity to simulate a disconnected update (which is typical in APIs)
            _context.Entry(address).State = EntityState.Detached;

            var updatedAddress = new Address { Id = 1, Street = "Updated Street", City = "Updated City", State = "Updated State", ZipCode = "12346", HouseNumber = "2A"};

            // Act
            await _userService.UpdateAddressAsync(updatedAddress);

            // Assert
            var dbAddress = await _context.Addresses.FindAsync(1);
            Assert.NotNull(dbAddress);
            Assert.Equal("Updated Street", dbAddress.Street);
        }

        [Fact]
        public async Task UpdateAddressAsync_NullAddress_ThrowsException()
        {
            // Arrange
            Address nullAddress = null!;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() =>
                _userService.UpdateAddressAsync(nullAddress));

            Assert.Equal("Address cannot be null", exception.Message);
        }
    }
}