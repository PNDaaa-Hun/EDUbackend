using EDUBackEnd.Models.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EDUBackEnd.Interfaces.User
{
    public interface IAdminService
    {
        // Get All
        Task<List<Student>> GetStudentsAsync();
        Task<List<Teacher>> GetTeachersAsync();
        Task<List<Admin>> GetAdminsAsync();
        Task<List<Models.Users.School>> GetSchoolsAsync();

        // Get by Id
        Task<Student> GetStudentByIdAsync(string studentId);
        Task<Teacher> GetTeacherByIdAsync(int teacherId);
        Task<Admin> GetAdminByIdAsync(int adminId);
        Task<Models.Users.School> GetSchoolByIdAsync(int schoolId);

        // Create
        Task CreateAdminAsync(Admin admin);
        Task<Student> CreateStudentAsync(Student student);
        Task<string> GenerateUniqueStudentIdAsync();
        Task CreateTeacherAsync(Teacher teacher);
        Task CreateSchoolAsync(Models.Users.School school);

        // Update
        Task UpdateStudentAsync(Student student);
        Task UpdateTeacherAsync(Teacher teacher);
        Task UpdateAdminAsync(Admin admin);
        Task UpdateSchoolAsync(Models.Users.School school);

        // Delete
        Task DeleteStudentAsync(string studentId);
        Task DeleteTeacherAsync(int teacherId);
        Task DeleteAdminAsync(int adminId);
        Task DeleteSchoolAsync(int schoolId);
    }
}
