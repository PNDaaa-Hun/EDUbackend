namespace EDUBackEnd.Dtos.Update
{
    public class UpdateScheduleDto
    {
        public int? TeacherId { get; set; }
        public int? SchoolClassId { get; set; }
        public int? SubjectId { get; set; }
        public int? TimeSlotId { get; set; }
        public bool? IsCancelled { get; set; } = false!;
        public bool? IsTeacherAbsence { get; set; } = false!;
        public int? ReplaceTeacherId { get; set; }
        public bool? IsExam { get; set; } = false!;
    }
}
