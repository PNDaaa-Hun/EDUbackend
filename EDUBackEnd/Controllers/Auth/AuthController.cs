using EDUBackEnd.Dtos.Reset;
using EDUBackEnd.Models.Auth;
using EDUBackEnd.Services.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EDUBackEnd.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService) 
        {
            _authService = authService;
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswrordRequest request)
        {
            var sent = await _authService.SendPasswordResetEmail(request.Email);
            if(!sent)
                return NotFound("No email was found");
            return Ok("Email was sent for reset your password");
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto request)
        {
            var ok = await _authService.ResetPasswordAsync(request.Token!, request.NewPassword, request.NewConfirmPassword);
            if (!ok)
                return BadRequest("The token was invalid or expired");
            return Ok("Password was changed successfully.");
        }
    }
}
