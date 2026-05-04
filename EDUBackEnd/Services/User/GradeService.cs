using EDUBackEnd.Data;
using EDUBackEnd.Dtos.Adding;
using EDUBackEnd.Dtos.List;
using EDUBackEnd.Enums;
using EDUBackEnd.Interfaces.User;
using EDUBackEnd.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace EDUBackEnd.Services.User
{
    public class GradeService : IGradeService
    {
        private readonly AppDbContext _context;
        public GradeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddGradesAsync(AddGradeDto dto)
        {
            if (dto.Value <1 || dto.Value > 5)
                throw new ArgumentException("Grade value must be between 1 and 5.");
            var student = await _context.Students.FindAsync(dto.StudentId);
            if (student == null)
                throw new ArgumentException("Student not found.");
            var grade = new Grade
            {
                StudentId = dto.StudentId,
                SubjectId = dto.SubjectId,
                Value = dto.Value,
                GradeType = dto.GradeType,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Grades.AddAsync(grade);

            student.Grades?.Add(grade);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteGrades(int id)
        {
            Grade? grade = await GetGradesById(id);
            _context.Grades.Remove(grade);
            await _context.SaveChangesAsync();
        }

        public async Task<Grade> GetGradesById(int id)
        {
            Grade? grade = await _context.Grades.FindAsync(id); 
            if (grade == null)
                    throw new ArgumentException("Grade not found.");
            return grade;
        }

        public async Task<List<Grade>> GetGrades()
        {
            return await _context.Grades.ToListAsync();
        }

        public async Task UpdateGrades(Grade grade)
        {
            if(grade.Value < 1 || grade.Value > 5)
                throw new ArgumentException("Grade value must be between 1 and 5.");
            _context.Grades.Update(grade);
            await _context.SaveChangesAsync();

        }

        public async Task<List<Grade>> GetGradesByStudentAndSubjectAsync(string studentId, int subjectId)
        {
            return await _context.Grades.Include(g => g.Subject)
                .Where(g => g.StudentId == studentId && g.SubjectId == subjectId)
                .OrderByDescending(g => g.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<StudentListDto>> GetStudentsByClassAsync(int schoolClassId)
        {
            return await _context.Students
                .Where(s => s.SchoolClassId == schoolClassId)
                .Select(s => new StudentListDto
                {
                    Id = s.Id,
                    FirstName = s.FirstName,
                    LastName = s.LastName
                })
                .ToListAsync();
        }

        public async Task<List<Grade>> GetGradesByStudentIdAsync(string studentId)
        {
            return await _context.Grades.Where(g => g.StudentId == studentId)
                .OrderByDescending(g => g.CreatedAt)
                .ToListAsync();
        }

        public async Task<Student> GetStudentByIdAsync(string studentId)
        {
            return await _context.Students.FirstOrDefaultAsync(s => s.Id == studentId);
        }
    }
}
