using EDUBackEnd.Data;
using EDUBackEnd.Dtos.Update;
using EDUBackEnd.Models.Timetable;
using Microsoft.EntityFrameworkCore;
using Xunit.Sdk;

namespace EDUBackEnd.Services.Timetable
{
    public class ScheduleService
    {
        private readonly AppDbContext _context;
        public ScheduleService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddEntryAsync(
            int schoolClassId,
            int subjectId,
            int teacherId,
            int timeSlotId)
        {
            var entry = new ScheduleEntry
            {
                SchoolId = _context.CurrentSchoolId,
                SchoolClassId = schoolClassId,
                SubjectId = subjectId,
                TeacherId = teacherId,
                TimeSlotId = timeSlotId
            };
            var conflict = await _context.ScheduleEntries.AnyAsync(e =>
                e.TimeSlotId == timeSlotId &&
                (e.TeacherId == teacherId ||
                e.SchoolClassId == schoolClassId));
            if (conflict)
            {
                string error = "";
                throw new Exception("Schedule conflict detected" + error);
            }
            await _context.ScheduleEntries.AddAsync(entry);
            await _context.SaveChangesAsync();
        }
        public async Task<List<ScheduleEntry>> GetScheduleAsync()
        {
            Console.WriteLine($"current School Id = {_context.CurrentSchoolId}");

            return await _context.ScheduleEntries
                .ToListAsync();
        }
        public async Task<List<ScheduleEntry>> GetClassScheduleAsync(int schoolClassId)
        {
            return await _context.ScheduleEntries
                .Where(e => e.SchoolClassId == schoolClassId)
                .ToListAsync();
        }
        public async Task<List<ScheduleEntry>> GetTeacherScheduleAsync(int teacherId)
        {
            return await _context.ScheduleEntries
                .Where(e => e.TeacherId == teacherId)
                .Include(e => e.SchoolClass)
                .Include(e => e.Subject)
                .Include(e => e.TimeSlot)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task DeleteEntryAsync(int scheduleEntryId)
        {
            var entry = await _context.ScheduleEntries
                .FirstOrDefaultAsync(e => e.Id == scheduleEntryId);

            if (entry == null)
                throw new Exception("Schedule entry not found.");

            _context.ScheduleEntries.Remove(entry);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateEntryAsync(ScheduleEntry scheduleEntry)
        {
            if (scheduleEntry is null)
                throw new Exception("Schedule entry cannot be null.");
            _context.ScheduleEntries.Update(scheduleEntry);
            await _context.SaveChangesAsync();
        }
        public async Task<ScheduleEntry> GetEntryByIdAsync(int scheduleEntryId)
        {
            ScheduleEntry entry = await _context.ScheduleEntries
                .FirstOrDefaultAsync(e => e.Id == scheduleEntryId);
            if (entry == null)
                throw new Exception("Schedule entry not found.");
            return entry;
        }
    }
}
