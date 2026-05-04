using EDUBackEnd.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EDUBackEnd.Services.School
{
    public class SchoolModelCacheKeyFactory : IModelCacheKeyFactory
    {
        public object Create(DbContext context, bool designTime)
        {
            if(context is AppDbContext schoolContext)
            {
                return (context.GetType(), schoolContext.CurrentSchoolId, designTime);
            }
            return (context.GetType(), designTime);
        }
    }
}
