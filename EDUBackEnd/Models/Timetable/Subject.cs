using EDUBackEnd.Interfaces.Timetable;

namespace EDUBackEnd.Models.Timetable
{
    public class Subject : ISchoolEntity
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int SchoolId { get; set; }
    }
}
