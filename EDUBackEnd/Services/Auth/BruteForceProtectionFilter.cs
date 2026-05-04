using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EDUBackEnd.Services.Auth
{
    public class BruteForceProtectionFilter : IAsyncActionFilter
    {
        private readonly BruteForceProtectionService _bruteForceProtectionService;
        public BruteForceProtectionFilter(BruteForceProtectionService bruteForceProtectionService)
        {
            _bruteForceProtectionService = bruteForceProtectionService;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var httpContext = context.HttpContext;

            var ip = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() 
                ?? httpContext.Connection.RemoteIpAddress?.ToString() 
                ?? "unknown";

            if (_bruteForceProtectionService.IsBlocked(ip))
            {
                context.Result = new ObjectResult("Request rejected.")
                {
                    StatusCode = StatusCodes.Status429TooManyRequests
                };
                return;
            }
            var executedContext = await next();

            if (executedContext.Result is ObjectResult result)
            {
                if (result.StatusCode == 401 || result.StatusCode == 400)
                {
                    _bruteForceProtectionService.RegisterFailedAttempt(ip);
                }
                else if (result.StatusCode is >= 200 and < 300)
                {
                    _bruteForceProtectionService.Reset(ip);
                }
            }
        }
    }
}
