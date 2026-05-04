namespace EDUBackEnd.Dtos.TimeTable
{
    public class CreateScheduleDto
    {
        public int SchoolClassId { get; set; }
        public int SubjectId { get; set; }
        public int TeacherId { get; set; }
        public int TimeSlotId { get; set; }
    }
}
