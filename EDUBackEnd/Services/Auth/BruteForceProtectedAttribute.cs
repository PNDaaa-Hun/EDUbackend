using Microsoft.AspNetCore.Mvc;

namespace EDUBackEnd.Services.Auth
{
    public class BruteForceProtectedAttribute : TypeFilterAttribute
    {
        public BruteForceProtectedAttribute() : base(typeof(BruteForceProtectionFilter))
        {
        }
    }
}
