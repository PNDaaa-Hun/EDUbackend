using EDUBackEnd.Dtos.Adding;
using EDUBackEnd.Dtos.List;
using EDUBackEnd.Models.Users;

namespace EDUBackEnd.Interfaces.User
{
    public interface IGradeService
    {
        Task AddGradesAsync(AddGradeDto dto);
        Task<List<Grade>> GetGrades();
        Task DeleteGrades(int id);
        Task UpdateGrades(Grade grade);
        Task<Grade> GetGradesById(int id); /// for delete and update
        Task<List<Grade>> GetGradesByStudentAndSubjectAsync(string studentId, int subjectId);
        Task<List<StudentListDto>> GetStudentsByClassAsync(int schoolClassId);
        Task<List<Grade>> GetGradesByStudentIdAsync(string studentId);
        Task<Student> GetStudentByIdAsync(string studentId);
    }
}
