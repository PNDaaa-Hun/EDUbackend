using EDUBackEnd.Interfaces.Timetable;

namespace EDUBackEnd.Models.Timetable
{
    public class SchoolClass : ISchoolEntity
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int SchoolId { get ; set ; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
    }
}
