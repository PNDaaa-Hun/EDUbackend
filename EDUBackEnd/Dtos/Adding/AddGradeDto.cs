using EDUBackEnd.Enums;

namespace EDUBackEnd.Dtos.Adding
{
    public class AddGradeDto
    {
        public string? StudentId { get; set; }
        public int? SubjectId { get; set; }
        public GradeType GradeType { get; set; }
        public int? Value { get; set; }
    }
}
