using EDUBackEnd.Interfaces.Timetable;
using EDUBackEnd.Models.Users;

namespace EDUBackEnd.Models.Timetable
{
    public class ScheduleEntry : ISchoolEntity
    {
        public int Id { get; set; }
        public int SchoolId { get; set; }
        public int SchoolClassId { get; set; }
        public SchoolClass? SchoolClass { get; set; }
        public int SubjectId { get; set; }
        public Subject? Subject { get; set; }
        public int TeacherId { get; set; }
        public Teacher? Teacher { get; set; }
        public int TimeSlotId { get; set; }
        public TimeSlot? TimeSlot { get; set; }
        public bool IsCancelled { get; set; } = false!;
        public bool IsTeacherAbsence { get; set; } = false!;
        public int? ReplaceTeacherId { get; set; }
        public Teacher ReplaceTeacher { get; set; }
        public bool IsExam { get; set; } = false!;
    }
}
