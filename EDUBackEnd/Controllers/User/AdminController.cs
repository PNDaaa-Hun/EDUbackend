using EDUBackEnd.Dtos.Adding;
using EDUBackEnd.Dtos.Update;
using EDUBackEnd.Interfaces.User;
using EDUBackEnd.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EDUBackEnd.Controllers.User
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IUserService _userService;
        public AdminController(IAdminService adminService, IUserService userService)
        {
            _adminService = adminService;
            _userService = userService;
        }

        [HttpGet("admins")]
        public async Task<IActionResult> GetAdminsAsnyc()
        {
            var admins = await _adminService.GetAdminsAsync();
            return Ok(admins);
        }

        [HttpGet("schools")]
        public async Task<IActionResult> GetSchoolsAsync()
        {
            var schools = await _adminService.GetSchoolsAsync();
            return Ok(schools);
        }
        [HttpGet("students")]
        public async Task<IActionResult> GetStudentsAsync()
        {
            var students = await _adminService.GetStudentsAsync();
            return Ok(students);
        }
        [HttpGet("teachers")]
        public async Task<IActionResult> GetTeachersAsync()
        {
            var teachers = await _adminService.GetTeachersAsync();
            return Ok(teachers);
        }
        [HttpPost("admins")]
        public async Task<IActionResult> CreateAdminAsync(AddAdminDto addAdminDto)
        {
            Admin admin = new Admin
            {
                FirstName = addAdminDto.FirstName,
                MiddleName = addAdminDto.MiddleName,
                LastName = addAdminDto.LastName,
                SchoolId = addAdminDto.SchoolId
            };
            await _adminService.CreateAdminAsync(admin);
            return Ok();
        }
        [HttpPost("schools")]
        public async Task<IActionResult> CreateSchoolAsync(AddSchoolDto addSchoolDto)
        {
            School school = new School
            {
                Name = addSchoolDto.Name,
                Country = addSchoolDto.Country,
                Region = addSchoolDto.Region,
                PostalCode = addSchoolDto.PostalCode,
                City = addSchoolDto.City,
                Address = addSchoolDto.Address
            };
            await _adminService.CreateSchoolAsync(school);
            return Ok();
        }
        [HttpPost("teachers")]
        public async Task<IActionResult> CreateTeacherAsync(AddTeacherDto addTeacherDto)
        {
            Teacher teacher = new Teacher
            {
                FirstName = addTeacherDto.FirstName,
                LastName = addTeacherDto.LastName,
                MiddleName = addTeacherDto.MiddleName,
                SchoolId = addTeacherDto.SchoolId,
            };
            await _adminService.CreateTeacherAsync(teacher);
            return Ok();
        }
        [HttpPost("students")]
        public async Task<IActionResult> CreateStudent([FromBody] AddStudentDto dto)
        {
            Student student = new Student
            {
                FirstName = dto.FirstName,
                MiddleName = dto.MiddleName,
                LastName = dto.LastName,
                SchoolId = dto.SchoolId
            };

            var createdStudent = await _adminService.CreateStudentAsync(student);

            return Ok(createdStudent);
        }
        [HttpDelete("schools/{schoolId}")]
        public async Task<IActionResult> DeleteSchoolAsync(int schoolId)
        {
            var school = await _adminService.GetSchoolByIdAsync(schoolId);
            if (school == null)
            {
                return NotFound("School not found");
            }
            await _adminService.DeleteSchoolAsync(schoolId);
            return Ok();
        }
        [HttpDelete("students/{studentId}")]
        public async Task<IActionResult> DeleteStudentAsync(string studentId)
        {
            var student = await _adminService.GetStudentByIdAsync(studentId);
            if (student == null)
            {
                return NotFound("Student not found");
            }
            await _adminService.DeleteStudentAsync(studentId);
            return Ok();
        }
        [HttpDelete("admins/{adminId}")]
        public async Task<IActionResult> DeleteAdminAsync(int adminId)
        {
            var admin = await _adminService.GetAdminByIdAsync(adminId);
            if (admin == null)
            {
                return NotFound("Admin not found");
            }
            await _adminService.DeleteAdminAsync(adminId);
            return Ok();
        }
        [HttpDelete("teachers/{teacherId}")]
        public async Task<IActionResult> DeleteTeacherAsync(int teacherId)
        {
            var teacher = await _adminService.GetTeacherByIdAsync(teacherId);
            if (teacher == null)
            {
                return NotFound("Teacher not found");
            }
            await _adminService.DeleteTeacherAsync(teacherId);
            return Ok();


        }
        [HttpGet("students/{studentId}")]
        public async Task<IActionResult> GetStudentByIdAsync(string studentId)
        {
            var student = await _adminService.GetStudentByIdAsync(studentId);
            if (student == null)
            {
                return NotFound("No Student was found with this Id");
            }
            return Ok(student.Id);
        }

        [HttpGet("teachers/{teacherId}")]
        public async Task<IActionResult> GetTeacherByIdAsync(int teacherId)
        {
            var teacher = await _adminService.GetTeacherByIdAsync(teacherId);
            if (teacher == null)
            {
                return NotFound("No Teacher was found with this Id");
            }
            return Ok(teacher);
        }
        [HttpGet("schools/{schoolId}")]
        public async Task<IActionResult> GetSchoolByIdAsync(int schoolId)
        {
            var school = await _adminService.GetSchoolByIdAsync(schoolId);
            if (school == null)
            {
                return NotFound("No School was found with this Id");
            }
            return Ok(school);
        }
        [HttpGet("admins/{adminId}")]
        public async Task<IActionResult> GetAdminByIdAsync(int adminId)
        {
            var admin = await _adminService.GetAdminByIdAsync(adminId);
            if (admin == null)
            {
                return NotFound("No Admin was found with this Id");
            }
            return Ok(admin);
        }
        [HttpPut("students/{id}")]
        public async Task<IActionResult> UpdateStudentAsync(string id, [FromBody] UpdateStudentDto? updatedStudent)
        {
            var existingStudent = await _adminService.GetStudentByIdAsync(id);
            if (existingStudent == null)
            {
                return NotFound("Student not found");
            }
            existingStudent.FirstName = updatedStudent!.FirstName! ?? existingStudent.FirstName;
            existingStudent.LastName = updatedStudent.LastName! ?? existingStudent.LastName!;
            existingStudent.MiddleName = updatedStudent.MiddleName! ?? existingStudent.MiddleName;
            existingStudent.SchoolId = updatedStudent.SchoolId;
            existingStudent.GradeId = updatedStudent.GradeId ?? existingStudent.GradeId;
            existingStudent.IsAbsence = updatedStudent.IsAbsence;
            existingStudent.SchoolClassId = updatedStudent.SchoolClassId ?? existingStudent.SchoolClassId;
            await _adminService.UpdateStudentAsync(existingStudent);
            return Ok();
        }
        [HttpPut("teachers/{id}")]
        public async Task<IActionResult> UpdateTeacherAsync(int id, [FromBody] UpdateTeacherDto updatedTeacher)
        {
            var existingTeacher = await _adminService.GetTeacherByIdAsync(id);
            if (existingTeacher == null)
            {
                return NotFound("Teacher not found");
            }
            existingTeacher.FirstName = updatedTeacher.FirstName! ?? existingTeacher.FirstName;
            existingTeacher.LastName = updatedTeacher.LastName! ?? existingTeacher.LastName;
            existingTeacher.MiddleName = updatedTeacher.MiddleName! ?? existingTeacher.MiddleName;
            existingTeacher.SchoolId = updatedTeacher.SchoolId;
            await _adminService.UpdateTeacherAsync(existingTeacher);
            return Ok();
        }
        [HttpPut("admins/{id}")]
        public async Task<IActionResult> UpdateAdminAsync(int id, [FromBody] UpdateAdminDto? updatedAdmin)
        {
            var existingAdmin = await _adminService.GetAdminByIdAsync(id);
            if (existingAdmin == null)
            {
                return NotFound("Admin not found");
            }
            existingAdmin.FirstName = updatedAdmin!.FirstName! ?? existingAdmin.FirstName;
            existingAdmin.LastName = updatedAdmin.LastName! ?? existingAdmin.LastName;
            existingAdmin.MiddleName = updatedAdmin.MiddleName! ?? existingAdmin.MiddleName;
            existingAdmin.SchoolId = updatedAdmin.SchoolId;
            await _adminService.UpdateAdminAsync(existingAdmin);
            return Ok();
        }
        [HttpPut("schools/{id}")]
        public async Task<IActionResult> UpdateSchoolAsync(int id, [FromBody] UpdateSchoolDto? updateSchoolDto)
        {
            var existingSchool = await _adminService.GetSchoolByIdAsync(id);
            if (existingSchool == null)
            {
                return NotFound("School not found");
            }
            existingSchool.Name = updateSchoolDto!.Name! ?? existingSchool.Name;
            await _adminService.UpdateSchoolAsync(existingSchool);
            return Ok();
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
        public async Task<IActionResult> UpdateAddressAsync(UpdateAddressDto addressDto, int addressId, string userId)
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

            var student = await _adminService.GetStudentByIdAsync(userId);
            if (student != null)
            {
                student.AddressId = addressId; // Link the student to the address
                await _adminService.UpdateStudentAsync(student);
            }

            await _userService.UpdateAddressAsync(existingAddress);
            return Ok();
        }
    }
}

