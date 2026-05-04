using EDUBackEnd.Data;
using EDUBackEnd.Dtos.Auth;
using EDUBackEnd.Enums;
using EDUBackEnd.Interfaces.Auth;
using EDUBackEnd.Interfaces.Email;
using EDUBackEnd.Interfaces.User;
using EDUBackEnd.Models.Users;
using EDUBackEnd.Services.Auth;
using EDUBackEnd.Services.Email;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EDUBackEnd.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<Models.Users.User> _userManager;
        private readonly SignInManager<Models.Users.User> _signInManager;
        private readonly AppDbContext _context;
        private readonly IAdminService _adminService;
        private readonly IForgotPassword _emailService;
        public AccountController(
            ITokenService tokenService,
            UserManager<Models.Users.User> userManager,
            SignInManager<Models.Users.User> signInManager,
            AppDbContext context,
            IAdminService adminService,
            IForgotPassword emailService)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _adminService = adminService;
            _emailService = emailService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto, [FromQuery] Role roles)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var user = new Models.Users.User
                {
                    UserName = dto.Email,
                    Email = dto.Email,
                    PhoneNumber = dto.Phone,
                    SchoolId = dto.SchoolId,
                    ClassId = dto.ClassId
                };

                var createdUser = await _userManager.CreateAsync(user, dto.Password);
                if (!createdUser.Succeeded)
                    return BadRequest(createdUser.Errors.FirstOrDefault()?.Description);

                var roleResult = await _userManager.AddToRoleAsync(user, roles.ToString());
                if (!roleResult.Succeeded)
                    return BadRequest("Couldn't assign role to the user");

                // Profil létrehozása a szerepkör alapján
                switch (roles)
                {
                    case Role.Admin:
                        var admin = new Admin
                        {
                            FirstName = dto.FirstName,
                            MiddleName = dto.MiddleName,
                            LastName = dto.LastName,
                            SchoolId = dto.SchoolId
                        };
                        await _adminService.CreateAdminAsync(admin);
                        user.AdminId = admin.Id;
                        break;

                    case Role.Teacher:
                        var teacher = new Teacher
                        {
                            FirstName = dto.FirstName,
                            MiddleName = dto.MiddleName,
                            LastName = dto.LastName,
                            SchoolId = dto.SchoolId,
                            
                        };
                        await _adminService.CreateTeacherAsync(teacher);
                        user.TeacherId = teacher.Id;
                        break;

                    case Role.Student:
                        var student = new Student
                        {
                            FirstName = dto.FirstName,
                            MiddleName = dto.MiddleName,
                            LastName = dto.LastName,
                            SchoolId = dto.SchoolId,
                            SchoolClassId = dto.ClassId
                        };
                        var createdStudent = await _adminService.CreateStudentAsync(student);
                        user.StudentId = createdStudent.Id;
                        break;
                }

                await _userManager.UpdateAsync(user);

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmUrl = Url.Action(
                    "ConfirmEmail",               
                    "Account",                    
                    new { userId = user.Id, token },
                    Request.Scheme
                );

                await _emailService.SendEmailAsync(
                    user.Email!,
                    "Confirm your email",
                    $"Hello {dto.FirstName},<br/>Please confirm your account by clicking <a href='{confirmUrl}'>here</a>."
                );

                await transaction.CommitAsync();

                return Ok(new
                {
                    Message = "A regisztráció sikeres. Kérjük, ellenőrizze az e-mailjét a fiókja megerősítéséhez."
                });
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return BadRequest("Invalid User ID");
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
                return BadRequest("Email confirmation failed");
            return Ok("Email confirmed successfully");
        }
        [Authorize]
        [HttpPost("2fa/email/enable")]
        public async Task<IActionResult> EnableEmail2fa()
        {
            var user = await _userManager.GetUserAsync(User);

            await _userManager.SetTwoFactorEnabledAsync(user!, true);
            user!.TwoFactorMethod = TwoFactorMethod.Email;
            await _userManager.UpdateAsync(user);

            return Ok(new { enabled = true });
        }
        [BruteForceProtected]
        [HttpPost("login/2fa/email")]
        public async Task<IActionResult> LoginWith2FAEmail(LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null)
                return Unauthorized("Invalid Email or Password");
            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if (!result.Succeeded)
                return Unauthorized("Invalid Email or Password");
            var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
            await _emailService.SendEmailAsync(user.Email!,
                "Your 2FA Code",
                $"Your two-factor authentication code is: {token}");
            return Ok("2FA code sent to your email.");
        }
        [BruteForceProtected]
        [HttpPost("login/2fa/email/verify")]
        public async Task<IActionResult> Verify2FAEmail(Verify2FAEmail dto)
        {
            
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null)
            {
                return Unauthorized("Invalid Email or Password");
            }
            var isValid = await _userManager.VerifyTwoFactorTokenAsync(user, "Email", dto.Code);
            if (!isValid)
            {
                return Unauthorized("Invalid 2FA code.");
            }
            return Ok(new LoggerUserDto
            {
                UserName = user.UserName!,
                Email = user.Email!,
                Role = (await _userManager.GetRolesAsync(user)).First()!,
                Token = await _tokenService.CreateToken(user)
            });
        }
        [Authorize]
        [HttpPost("2fa/disable")]
        public async Task<IActionResult> Disable2fa()
        {
            var user = await _userManager.GetUserAsync(User);

            await _userManager.SetTwoFactorEnabledAsync(user!, false);
            user!.TwoFactorMethod = TwoFactorMethod.None;
            await _userManager.UpdateAsync(user);

            return Ok();
        }

        
        [BruteForceProtected]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if(user == null)
            {
                return Unauthorized("Invalid Email or Password");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if(!result.Succeeded)
            {
                return Unauthorized("Invalid Email or Password");
            }
            if (user.TwoFactorEnabled)
            {
                return Ok(new
                {
                    requiredTwoFactor = true,
                    method = user.TwoFactorMethod
                });
            }

            return Ok(new LoggerUserDto
            {
                UserName = user.UserName!,
                Email = user.Email!,
                Role = (await _userManager.GetRolesAsync(user)).First()!,
                Token = await _tokenService.CreateToken(user)
            });
        }
        [BruteForceProtected]
        [HttpPost("studentlogin")]
        public async Task<IActionResult> StudentLogin(LoginDtoStudent dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.Users.Include(u => u.Student).FirstOrDefaultAsync(u => u.StudentId == dto.Id.ToUpper());
            if(user == null)
            {
                return Unauthorized("Invalid Student ID or Password");
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if (!result.Succeeded)
            {
                return Unauthorized("Invalid Student ID or Password");
            }


            if (user.TwoFactorEnabled)
                return Ok(new { RequiresTwoFactor = true , method = user.TwoFactorMethod});

            return Ok(new LoggerUserDto
            {
                UserName = user.UserName!,
                Email = user.Email!,
                Role = (await _userManager.GetRolesAsync(user)).First()!,
                SchoolClassId = user.Student.SchoolClassId ?? null, 
                Token = await _tokenService.CreateToken(user)
            });
        }
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> LogOut()
        {
            bool logoutStatus = _signInManager.SignOutAsync().IsCompletedSuccessfully;
            if (!logoutStatus)
                return BadRequest("Error during logout.");

            return Ok(true);
        }
    }
}
