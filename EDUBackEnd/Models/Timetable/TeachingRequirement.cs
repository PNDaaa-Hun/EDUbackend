using EDUBackEnd.Interfaces.Timetable;

namespace EDUBackEnd.Models.Timetable
{
    public class TeachingRequirement : ISchoolEntity
    {
        public int Id { get; set; }
        public required int TeacherId { get; set; }
        public required int SubjectId { get; set; }
        public int? SchoolClassId { get; set; }
        public required int WeeklyLessons { get; set; }
        public int SchoolId { get; set ; }
    }
}
