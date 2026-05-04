using EDUBackEnd.Data;
using EDUBackEnd.Models.Timetable;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Storage.Internal.Json;

namespace EDUBackEnd.Services.School
{
    public class SchoolDataService
    {
        private readonly AppDbContext _context;
        public SchoolDataService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Subject> AddSubjectAsync(string name)
        {
            var subject = new Subject
            {
                SchoolId = _context.CurrentSchoolId,
                Name = name
            };
            await _context.Subjects.AddAsync(subject);
            await _context.SaveChangesAsync();
            return subject;
        }
        public async Task<List<Subject>> GetSubjectsAsync()
        {
            return await _context.Subjects
                .Where(s => s.SchoolId == _context.CurrentSchoolId)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<SchoolClass> AddClassAsync(string name)
        {
            var classes = new SchoolClass
            {
                Name = name,
                SchoolId = _context.CurrentSchoolId,
                CreatedAt = DateTime.UtcNow,
            };

            await _context.SchoolClasses.AddAsync(classes);
            await _context.SaveChangesAsync();

            return classes;
        }

        public async Task<SchoolClass> GetClassByIdAsync(int id)
        {
            return await _context.SchoolClasses
                .Where(c => c.SchoolId == _context.CurrentSchoolId && c.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<SchoolClass> UpdateClassAsync(int id, string name)
        {
            var classes = await _context.SchoolClasses
                .Where(c => c.SchoolId == _context.CurrentSchoolId && c.Id == id)
                .FirstOrDefaultAsync();
            if (classes == null)
                return null;
            classes.Name = name;
            await _context.SaveChangesAsync();
            return classes;
        }
        /// Delete class (could delete student within class)
        public async Task<bool> DeleteClassAsync(int id)
        {
            var classes = await _context.SchoolClasses
                .Where(c => c.SchoolId == _context.CurrentSchoolId && c.Id == id)
                .FirstOrDefaultAsync();
            if (classes == null)
                return false;
            _context.SchoolClasses.Remove(classes);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<SchoolClass>> GetClassesAsync()
        {
            return await _context.SchoolClasses
                .Where(c => c.SchoolId == _context.CurrentSchoolId)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<TimeSlot> AddTimeSlotAsync(DayOfWeek day, int lessonNumber)
        {
            var slot = new TimeSlot
            {
                Day = day,
                LessonNumber = lessonNumber,
                SchoolId = _context.CurrentSchoolId
                
            };

            await _context.TimeSlots.AddAsync(slot);
            await _context.SaveChangesAsync();

            return slot;
        }

        public async Task<List<TimeSlot>> GetTimeSlotsAsync()
        {
            return await _context.TimeSlots
                .Where(t => t.SchoolId == _context.CurrentSchoolId)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<TeachingRequirement> TeachingRequirementAsync(int subjectId,
            int weeklyLessons,
            int teacherId,
            int schoolClassId)
        {
            var teachingRequirement = new TeachingRequirement
            {
                SchoolId = _context.CurrentSchoolId,
                SubjectId = subjectId,
                WeeklyLessons = weeklyLessons,
                TeacherId = teacherId,
                SchoolClassId = schoolClassId,
            };
            await _context.TeachingRequirements.AddAsync(teachingRequirement);
            await _context.SaveChangesAsync();

            return teachingRequirement;
        }
        public async Task<List<TeachingRequirement>> GetTeachingRequirementsAsync()
        {
            return await _context.TeachingRequirements
                .Where(r => r.SchoolId == _context.CurrentSchoolId)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}