using EDUBackEnd.Data;
using EDUBackEnd.Models.Timetable;
using Microsoft.EntityFrameworkCore;
using MimeKit.Cryptography;

namespace EDUBackEnd.Services.Timetable
{
    public class TimeTableGenerateService
    {
        private readonly AppDbContext _context;
        private readonly ScheduleService _scheduleService;
        public TimeTableGenerateService(AppDbContext context,
            ScheduleService scheduleService)
        {
            _context = context;
            _scheduleService = scheduleService;
        }
        public async Task<bool> HasConflictAsync(ScheduleEntry entry)
        {
            return await _context.ScheduleEntries.AnyAsync(e =>
                e.TimeSlot == entry.TimeSlot &&
                (e.Teacher == entry.Teacher ||
                e.SchoolClass == entry.SchoolClass));
        }
        public async Task GenerateTimeTable()
        {
            // 1. Validation
            if (_context.CurrentSchoolId == 0)
                throw new Exception("No active school context.");

            // 2. Load Data
            var requirements = await _context.TeachingRequirements
                .Where(r => r.SchoolId == _context.CurrentSchoolId)
                .ToListAsync();

            var timeSlots = await _context.TimeSlots
                .Where(t => t.SchoolId == _context.CurrentSchoolId)
                .OrderBy(t => t.Day)
                .ThenBy(t => t.StartTime)
                .ToListAsync();

            if (!requirements.Any()) throw new Exception("No teaching requirements found.");
            if (!timeSlots.Any()) throw new Exception("No time slots defined for this school.");

            // 3. Start Transaction to ensure data integrity
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 4. Optional: Clear existing schedule for this school before generating new one
                var existingEntries = _context.ScheduleEntries
                    .Where(e => e.SchoolId == _context.CurrentSchoolId);

                _context.ScheduleEntries.RemoveRange(existingEntries);
                await _context.SaveChangesAsync();

                // 5. Allocation Loop
                foreach (var req in requirements)
                {
                    int assignedCount = 0;

                    foreach (var slot in timeSlots)
                    {
                        if (assignedCount >= req.WeeklyLessons)
                            break;

                        try
                        {
                            // We use the ScheduleService to handle the specific conflict logic and saving
                            var entry = new ScheduleEntry
                            {
                                SchoolId = _context.CurrentSchoolId,
                                SchoolClassId = req.SchoolClassId.Value,
                                SubjectId = req.SubjectId,
                                TeacherId = req.TeacherId,
                                TimeSlotId = slot.Id,
                            };

                            _context.ScheduleEntries.Add(entry);
                            await _context.SaveChangesAsync();

                            assignedCount++;
                        }
                        catch (Exception ex) when (ex.Message.Contains("conflict"))
                        {
                           
                            throw new Exception($"Error assigning {req.SubjectId} to Class {req.SchoolClassId} at TimeSlot {slot.Id}: {ex.Message}");
                            continue;
                        }
                    }

                    // 6. Verification
                    //if (assignedCount < req.WeeklyLessons)
                    //{
                    //    throw new Exception($"Failed to generate timetable: Could only assign {assignedCount}/{req.WeeklyLessons} " +
                    //                        $"lessons for ClassID {req.SchoolClassId}, SubjectID {req.SubjectId}.");
                    //}
                }

                // 7. Commit everything to the database
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                // If anything goes wrong, nothing is saved to the database
                await transaction.RollbackAsync();
                throw;
            }
            var count = await _context.ScheduleEntries.CountAsync();
            Console.WriteLine($"DB COUNT: {count}");

            var realCount = await _context.ScheduleEntries
                .IgnoreQueryFilters()
                .CountAsync();
            Console.WriteLine($"REAL COUNT: {realCount}");
        }
    }
}
