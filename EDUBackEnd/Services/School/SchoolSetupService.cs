using EDUBackEnd.Data;
using EDUBackEnd.Models.Timetable;
using Microsoft.EntityFrameworkCore;

namespace EDUBackEnd.Services.School
{
    public class SchoolSetupService
    {
        private readonly AppDbContext _context;

        public SchoolSetupService(AppDbContext context)
        {
            _context = context;
        }

        public async Task SeedBasicDataAsync()
        {
            if (_context.CurrentSchoolId == 0)
                throw new Exception("No active school context.");

            var firstLessonStart = new TimeSpan(7, 45, 0);
            var lessonDuration = TimeSpan.FromMinutes(45);
            var breakDuration = TimeSpan.FromMinutes(10);   

            if (!await _context.TimeSlots.AnyAsync())
            {
                var days = new[]
                {
                    DayOfWeek.Monday,
                    DayOfWeek.Tuesday,
                    DayOfWeek.Wednesday,
                    DayOfWeek.Thursday,
                    DayOfWeek.Friday
               };

                foreach (var day in days)
                {
                    for (int lesson = 1; lesson <= 16; lesson++)
                    {
                        var startTime = firstLessonStart + TimeSpan.FromMinutes((lesson - 1) * (45+10));
                        var endTime = startTime + lessonDuration;
                        _context.TimeSlots.Add(new TimeSlot
                        {
                            SchoolId = _context.CurrentSchoolId,
                            Day = day,
                            LessonNumber = lesson,
                            StartTime = startTime,
                            EndTime = endTime
                        });
                    }
                }
            }

            // 🔹 Tantárgyak
            if (!await _context.Subjects.AnyAsync())
            {
                var subjects = new[] { "Math", "History", "Biology", "Physics", "English" };

                foreach (var name in subjects)
                {
                    _context.Subjects.Add(new Subject
                    {
                        SchoolId = _context.CurrentSchoolId,
                        Name = name
                    });
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
