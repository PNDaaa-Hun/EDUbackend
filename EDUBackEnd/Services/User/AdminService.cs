using EDUBackEnd.Data;
using EDUBackEnd.Interfaces.User;
using EDUBackEnd.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EDUBackEnd.Services.User
{
    public class AdminService : IAdminService
    {
        readonly string IdNotFound = "No id was found";
        private readonly AppDbContext _context;
        private readonly UserManager<Models.Users.User> _userManager;

        public AdminService(AppDbContext context,
            UserManager<Models.Users.User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task CreateAdminAsync(Admin admin)
        {
            await _context.Admins.AddAsync(admin);
            await _context.SaveChangesAsync();
        }

        public async Task CreateSchoolAsync(Models.Users.School school)
        {
            await _context.Schools.AddAsync(school);
            await _context.SaveChangesAsync();
        }

        public async Task<string> GenerateUniqueStudentIdAsync()
        {
            string newId;
            do
            {
                newId = StudentIdGenerator.GenerateId();
            }
            while (await _context.Students.AnyAsync(s => s.Id == newId));

            return newId;
        }

        public async Task<Student> CreateStudentAsync(Student student)
        {
            student.Id = await GenerateUniqueStudentIdAsync();
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task CreateTeacherAsync(Teacher teacher)
        {
            await _context.Teachers.AddAsync(teacher);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAdminAsync(int adminId)
        {
            Admin admin = await GetAdminByIdAsync(adminId);
            _context.Admins.Remove(admin);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSchoolAsync(int schoolId)
        {
            Models.Users.School school = await GetSchoolByIdAsync(schoolId);
            _context.Schools.Remove(school);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteStudentAsync(string studentId)
        {
            Student student = await GetStudentByIdAsync(studentId);
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.StudentId == student.Id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
            _context.Students.Remove(student);
            
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTeacherAsync(int teacherId)
        {
            Teacher teacher = await GetTeacherByIdAsync(teacherId);
            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();
        }

        public async Task<Admin> GetAdminByIdAsync(int adminId)
        {
            Admin? admin = await _context.Admins.FirstOrDefaultAsync(a => a.Id == adminId);
            if (admin is null)
                throw new Exception(IdNotFound);
            return admin;
        }

        public async Task<List<Admin>> GetAdminsAsync()
        {
            return await _context.Admins.AsNoTracking().ToListAsync();
        }

        public async Task<List<Models.Users.School>> GetSchoolsAsync()
        {
            return await _context.Schools.AsNoTracking().ToListAsync();
        }

        public async Task<List<Student>> GetStudentsAsync()
        {
            return await _context.Students.AsNoTracking().ToListAsync();
        }

        public async Task<List<Teacher>> GetTeachersAsync()
        {
            return await _context.Teachers.AsNoTracking().ToListAsync();
        }

        public async Task<Models.Users.School> GetSchoolByIdAsync(int schoolId)
        {
            Models.Users.School? school = await _context.Schools.FirstOrDefaultAsync(s => s.Id == schoolId);
            if (school is null)
                throw new Exception(IdNotFound);
            return school;
        }

        public async Task<Student> GetStudentByIdAsync(string studentId)
        {
            Student? student = await _context.Students.FirstOrDefaultAsync(s => s.Id == studentId);
            if (student is null)
                throw new Exception(IdNotFound);
            return student;
        }

        public async Task<Teacher> GetTeacherByIdAsync(int teacherId)
        {
            Teacher? teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == teacherId);
            if (teacher is null)
                throw new Exception(IdNotFound);
            return teacher;
        }

        public async Task UpdateAdminAsync(Admin admin)
        {
            if (admin is null)
                throw new Exception("Admin object is null");

            _context.Admins.Update(admin);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSchoolAsync(Models.Users.School school)
        {
            if (school is null)
                throw new Exception("School object is null");

            _context.Schools.Update(school);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateStudentAsync(Student student)
        {
            if (student is null)
                throw new Exception("Student object is null");

            _context.Students.Update(student);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTeacherAsync(Teacher teacher)
        {
            if (teacher is null)
                throw new Exception("Teacher object is null");

            _context.Teachers.Update(teacher);
            await _context.SaveChangesAsync();
        }
    }
}
