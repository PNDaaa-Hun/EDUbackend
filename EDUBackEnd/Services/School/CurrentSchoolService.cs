using EDUBackEnd.Interfaces.Timetable.School;

namespace EDUBackEnd.Services.Timetable.School
{
    public class CurrentSchoolService : ICurrentSchoolService
    {
        private readonly HttpContextAccessor _httpContextAccessor;
        public CurrentSchoolService(HttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public int SchoolId
        {
            get
            {
                var httpContext = _httpContextAccessor.HttpContext;

                if (httpContext == null)
                    return 0; // design-time fallback

                var claim = httpContext.User.FindFirst("SchoolId");

                if (claim == null)
                    return 0;

                return int.Parse(claim.Value);
            }
        }
    }
}
