using EDUBackEnd.Enums;

namespace EDUBackEnd.Dtos.Update
{
    public class UpdateGradeDto
    {
        public string? StudentId { get; set; }
        public int? SubjectId { get; set; }
        public GradeType GradeType { get; set; }
        public int? Value { get; set; }
    }
}
