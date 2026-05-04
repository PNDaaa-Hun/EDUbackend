namespace EDUBackEnd.Dtos.Adding
{
    // ==============================
    // ASSIGNMENT DTOs
    // ==============================
    public class AssingmentCreateDto
    {
        public int CourseId { get; set; }
        public int RoomId { get; set; }
        public int TimeSlotId { get; set; }
    }

    public class SimpleAssingmentDto
    {
        public int Id { get; set; }
        public SimpleCourseDto Course { get; set; }
        public SimpleRoomDto Room { get; set; }
        public SimpleTimeSlotDto TimeSlot { get; set; }
    }

    // ==============================
    // COURSE DTOs
    // ==============================
    public class CourseCreateDto
    {
        public string Name { get; set; } = null!;
        public int TeacherId { get; set; }
        public int StudentCount { get; set; }
        public int LessonCount { get; set; }
        public List<int>? PreferredTimeSlotIds { get; set; }
        public int StudentGroupId { get; set; }
    }

    public class SimpleCourseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string TeacherName { get; set; } = null!;
        public string StudentGroup { get; set; } = null!;
    }

    // ==============================
    // ROOM DTOs
    // ==============================
    public class RoomCreateDto
    {
        public string Name { get; set; } = null!;
        public int Capacity { get; set; }
        public int SchoolId { get; set; }
    }

    public class SimpleRoomDto
    {
        public string Name { get; set; } = null!;
    }

    // ==============================
    // TIMESLOT DTOs
    // ==============================
    public class TimeSlotCreateDto
    {
        public DayOfWeek Day { get; set; }
        public TimeOnly Start { get; set; }
        public TimeOnly End { get; set; }
    }

    public class SimpleTimeSlotDto
    {
        public string Day { get; set; } = null!;
        public string Start { get; set; } = null!;
        public string End { get; set; } = null!;
    }
}
