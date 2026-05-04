using EDUBackEnd.Dtos.Adding;
using EDUBackEnd.Dtos.Update;
using EDUBackEnd.Interfaces.User;
using EDUBackEnd.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EDUBackEnd.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("address")]
        public async Task<IActionResult> AddAddressAsync(AddAddressDto addAddressDto)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized("User not found.");
            }
            Address address = new Address
            {
                Street = addAddressDto.Street,
                City = addAddressDto.City,
                State = addAddressDto.State,
                ZipCode = addAddressDto.ZipCode,
                HouseNumber = addAddressDto.HouseNumber
            };
            await _userService.AddAddressAsync(address, userId);
            return Ok();
        }
        [HttpGet("address/{id}")]
        public async Task<IActionResult> GetAddressByIdAsync(int id)
        {
            var address = await _userService.GetAddressByIdAsync(id);
            return Ok(address);
        }
        [HttpGet("addresses")]
        public async Task<IActionResult> GetAddresses()
        {
            var addresses = await _userService.GetAddresses();
            return Ok(addresses);
        }
        [HttpPut("address/{addressId}")]
        public async Task<IActionResult> UpdateAddressAsync(UpdateAddressDto addressDto, int addressId)
        {
            var existingAddress = await _userService.GetAddressByIdAsync(addressId);
            if (existingAddress == null)
            {
                return NotFound("Address not found.");
            }
            existingAddress.State = addressDto.State ?? existingAddress.State;
            existingAddress.City = addressDto.City ?? existingAddress.City;
            existingAddress.Street = addressDto.Street ?? existingAddress.Street;
            existingAddress.ZipCode = addressDto.ZipCode ?? existingAddress.ZipCode;
            existingAddress.HouseNumber = addressDto.HouseNumber ?? existingAddress.HouseNumber;

            await _userService.UpdateAddressAsync(existingAddress);
            return Ok();
        }
    }
}
