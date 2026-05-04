using EDUBackEnd.Dtos.Adding;
using EDUBackEnd.Dtos.TimeTable;
using EDUBackEnd.Services.School;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EDUBackEnd.Controllers.Timetable
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class SchoolDataController : ControllerBase
    {
        private readonly SchoolDataService _dataService;

        public SchoolDataController(SchoolDataService dataService)
        {
            _dataService = dataService;
        }
        [Authorize(Roles = "TimeTableManager,Teacher,Admin")]
        [HttpPost("subjects")]
        public async Task<IActionResult> AddSubject([FromBody] CreateSubjectDto dto)
        {
            var s = await _dataService.AddSubjectAsync(dto.Name);
            return Ok(s);
        }

        [Authorize(Roles = "TimeTableManager,Teacher,Admin,Student")]
        [HttpGet("subjects")]
        public async Task<IActionResult> GetSubjects()
        {
            var list = await _dataService.GetSubjectsAsync();
            return Ok(list);
        }
        [Authorize(Roles = "TimeTableManager,Teacher,Admin")]
        [HttpPost("classes")]
        public async Task<IActionResult> AddClass([FromQuery] string name)
        {
            var c = await _dataService.AddClassAsync(name);
            return Ok(c);
        }
        [Authorize(Roles = "TimeTableManager,Teacher,Admin,Student")]
        [HttpGet("classes")]
        public async Task<IActionResult> GetClasses()
        {
            var list = await _dataService.GetClassesAsync();
            return Ok(list);
        }
        [Authorize(Roles = "TimeTableManager,Teacher,Admin")]
        [HttpPost("timeslots")]
        public async Task<IActionResult> AddTimeSlot([FromQuery] DayOfWeek day, [FromQuery] int lessonNumber)
        {
            var t = await _dataService.AddTimeSlotAsync(day, lessonNumber);
            return Ok(t);
        }
        [Authorize(Roles = "TimeTableManager,Teacher,Admin,Student")]
        [HttpGet("timeslots")]
        public async Task<IActionResult> GetTimeSlots()
        {
            var list = await _dataService.GetTimeSlotsAsync();
            return Ok(list);
        }
        [Authorize(Roles = "TimeTableManager,Teacher,Admin")]
        [HttpPost("teachingrequirments")]
        public async Task<IActionResult> AddTeachingRequirment([FromQuery] int subjectId,
            [FromQuery] int weeklyLessons,
            [FromQuery] int teacherId,
            [FromQuery] int schoolClassId)
        {
            var tr = await _dataService.TeachingRequirementAsync(subjectId, weeklyLessons, teacherId, schoolClassId);
            return Ok(tr);
        }
        [Authorize(Roles = "TimeTableManager,Teacher,Admin,Student")]
        [HttpGet("teachingrequirments")]
        public async Task<IActionResult> GetTeachingRequirments()
        {
            var list = await _dataService.GetTeachingRequirementsAsync();
            return Ok(list);
        }
    }
}
