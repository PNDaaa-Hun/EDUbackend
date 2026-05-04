using EDUBackEnd.Interfaces.Timetable;

namespace EDUBackEnd.Models.Timetable
{
    public class TimeSlot : ISchoolEntity
    {
        public int Id { get; set; }
        public required DayOfWeek Day { get; set; }
        public required int LessonNumber { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public int SchoolId { get ; set; }
    }
}
