using EDUBackEnd.Dtos.TimeTable;
using EDUBackEnd.Dtos.Update;
using EDUBackEnd.Enums;
using EDUBackEnd.Models.Users;
using EDUBackEnd.Services.School;
using EDUBackEnd.Services.Timetable;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EDUBackEnd.Controllers.Timetable
{
    [Route("api/[controller]")]
    [ApiController]

    public class TimeTableController : ControllerBase
    {
        private readonly ScheduleService _scheduleService;
        private readonly TimeTableGenerateService _generateService;
        private readonly SchoolSetupService _setupService;
        public TimeTableController(ScheduleService scheduleService,
            TimeTableGenerateService generateService,
            SchoolSetupService setupService)
        {
            _scheduleService = scheduleService;
            _generateService = generateService;
            _setupService = setupService;
        }
        [Authorize(Roles = "TimeTableManager,Teacher,Admin")]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateScheduleDto dto)
        {
            try
            {
                await _scheduleService.AddEntryAsync(
                    dto.SchoolClassId,
                    dto.SubjectId,
                    dto.TeacherId,
                    dto.TimeSlotId
                    );
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("table")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var schedule = await _scheduleService.GetScheduleAsync();
                return Ok(schedule);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "Teacher")]
        [HttpGet("teachertable")]
        public async Task<IActionResult> GetTeacherClasses(int teacherId)
        {
            try
            {
                var schedule = await _scheduleService.GetTeacherScheduleAsync(teacherId);
                return Ok(schedule);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "Student")]
        [HttpGet("studenttable")]
        public async Task<IActionResult> GetStudentClasses(int schoolClassId)
        {
            try
            {
                var schedule = await _scheduleService.GetClassScheduleAsync(schoolClassId);
                Console.WriteLine($"DB RETURNED : {schedule.Count} ROWS FOR CLASS {schoolClassId}");
                return Ok(schedule);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "TimeTableManager,Admin")]
        [HttpPost("generate")]
        public async Task<IActionResult> Generate()
        {
            try
            {
                await _generateService.GenerateTimeTable();
                return Ok("Timetable generated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "TimeTableManager,Admin")]
        [HttpPost("setup")]
        public async Task<IActionResult> SetupSchool()
        {
            await _setupService.SeedBasicDataAsync();
            return Ok("School initialized.");
        }
        [Authorize(Roles = "TimeTableManager,Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _scheduleService.DeleteEntryAsync(id);
                return Ok("Schedule entry deleted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin,Teacher,TimeTableManager")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateScheduleDto dto)
        {
            var existingSchedule = await _scheduleService.GetEntryByIdAsync(id);
            if (existingSchedule == null)
            {
                return NotFound("Schedule entry not found.");
            }
            existingSchedule.SchoolClassId = dto.SchoolClassId ?? existingSchedule.SchoolClassId;
            existingSchedule.SubjectId = dto.SubjectId ?? existingSchedule.SubjectId;
            existingSchedule.TeacherId = dto.TeacherId ?? existingSchedule.TeacherId;
            existingSchedule.TimeSlotId = dto.TimeSlotId ?? existingSchedule.TimeSlotId;
            existingSchedule.IsCancelled = dto.IsCancelled ?? existingSchedule.IsCancelled;
            existingSchedule.IsTeacherAbsence = dto.IsTeacherAbsence ?? existingSchedule.IsTeacherAbsence;
            existingSchedule.ReplaceTeacherId = dto.ReplaceTeacherId ?? existingSchedule.ReplaceTeacherId;
            existingSchedule.IsExam = dto.IsExam ?? existingSchedule.IsExam;
            await _scheduleService.UpdateEntryAsync(existingSchedule);
            return Ok("Schedule entry updated successfully.");
        }
    }
}
