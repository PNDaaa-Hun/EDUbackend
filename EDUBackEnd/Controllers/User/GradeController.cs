using EDUBackEnd.Data;
using EDUBackEnd.Dtos.Adding;
using EDUBackEnd.Dtos.Update;
using EDUBackEnd.Enums;
using EDUBackEnd.Interfaces.User;
using EDUBackEnd.Models.Users;
using EDUBackEnd.Services.User;
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
    public class GradeController : ControllerBase
    {
        private readonly IGradeService _gradeService;
        private readonly AppDbContext _context;
        private readonly UserManager<Models.Users.User> _userManager;
        public GradeController(IGradeService gradeService,
            AppDbContext context,
            UserManager<Models.Users.User> userManager)
        {
            _gradeService = gradeService;
            _context = context;
            _userManager = userManager;
        }
        [Authorize(Roles = "Teacher")]
        [HttpPost("grades")]
        public async Task<IActionResult> AddGrade(AddGradeDto dto)
        {
            try
            {
                await _gradeService.AddGradesAsync(dto);
                return Ok("Grade added successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [Authorize(Roles = "Teacher,Student")]
        [HttpGet("/grades")]
        public async Task<IActionResult> GetGrades()
        {
            var grades = _gradeService.GetGrades();
            return Ok(grades);
        }
        [Authorize(Roles = "Teacher")]
        [HttpDelete("grades/{id}")]
        public async Task<IActionResult> DeleteGrade(int id)
        {
            await _gradeService.DeleteGrades(id);
            return NoContent();
        }
        [Authorize(Roles = "Teacher")]
        [HttpPut("grades/{id}")]
        public async Task<IActionResult> UpdateGrade(int id, UpdateGradeDto gradeDto)
        {
            var existingGrade = await _gradeService.GetGradesById(id);
            if (existingGrade == null)
            {
                return NotFound("Grade not found.");
            }
            existingGrade.StudentId = gradeDto.StudentId ?? existingGrade.StudentId;
            existingGrade.SubjectId = gradeDto.SubjectId ?? existingGrade.SubjectId;
            existingGrade.GradeType = gradeDto.GradeType;
            existingGrade.Value = gradeDto.Value ?? existingGrade.Value;
            existingGrade.UpdatedAt = DateTime.UtcNow;
            await _gradeService.UpdateGrades(existingGrade);
            return Ok(existingGrade);
        }
        [Authorize(Roles = "Teacher")]
        [HttpPost("grades/{id}")]
        public async Task<IActionResult> GetGradeById(int id)
        {
            var grade = await _gradeService.GetGradesById(id);
            if (grade == null)
            {
                return NotFound("Grade not found.");
            }
            return Ok(grade);
        }

        [Authorize(Roles = "Student")]
        [HttpGet("my-grades/{subjectId}")]
        public async Task<IActionResult> GetMyGradesBySubject(int subjectId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            
            var userWithStudent = await _userManager.Users
                .Include(u => u.Student) 
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (userWithStudent?.Student == null)
            {
                return NotFound("No student profile is linked to this account.");
            }

            
            var studentId = userWithStudent.Student.Id;

            Console.WriteLine($"---> DATABASE SEARCH: Looking for StudentId: {studentId}");

            
            var grades = await _gradeService.GetGradesByStudentAndSubjectAsync(studentId, subjectId);

            return Ok(grades ?? new List<Grade>());
        }

        [Authorize(Roles = "Teacher")]
        [HttpGet("students-by-class/{schoolClassId}")]
        public async Task<IActionResult> GetStudentsByClass(int schoolClassId)
        {
            try
            {
                var students = await _gradeService.GetStudentsByClassAsync(schoolClassId);

                if (students == null || !students.Any())
                {
                    return NotFound("No students were found in this class");
                }

                return Ok(students);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "Teacher")]
        [HttpGet("studentgrades/{studentId}")]
        public async Task<IActionResult> GetGradesByStudentId(string studentId)
        {
            try
            {
                var grades = await _gradeService.GetGradesByStudentIdAsync(studentId);
                if (grades == null || !grades.Any())
                {
                    return NotFound("No grades were found for this student");
                }
                return Ok(grades);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "Teacher,Student")]
        [HttpGet("averagegrades")]
        public async Task<IActionResult> GetAverageGrades()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized("No user was found with this Id");
            var grades = await _gradeService.GetGradesByStudentIdAsync(userId);
            if (grades == null || !grades.Any())
            {
                return NotFound("No grades were found for this student");
            }
            var averageGrade = grades.Average(g => g.Value);
            return Ok(new { AverageGrade = averageGrade });
        }}
}
