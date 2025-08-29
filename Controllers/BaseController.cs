using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FumicertiApi.Controllers
{
    [Authorize]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected string GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.Name); // custom claim
            if (userIdClaim != null)
                return userIdClaim.Value;

            return null; // fallback (you can also throw exception if preferred)
        }

        protected int GetCompanyId()
        {
            var companyIdClaim = User.FindFirst("CompanyId");
            if (companyIdClaim != null && int.TryParse(companyIdClaim.Value, out int companyId))
                return companyId;

            return 0;
        }

        protected int GetBranchId()
        {
            var branchIdClaim = User.FindFirst("BranchId");
            if (branchIdClaim != null && int.TryParse(branchIdClaim.Value, out int branchId))
                return branchId;

            return 0;
        }


        protected string GetUserRole()
        {
            return User.FindFirst(ClaimTypes.Role)?.Value ?? "Unknown";
        }

        protected IQueryable<T> FilterByCompany<T>(IQueryable<T> query, string companyPropName)
        {
            var companyId = GetCompanyId();

            // Build lambda dynamically: e => EF.Property<int>(e, "companyPropName") == companyId
            return query.Where(e => EF.Property<int>(e, companyPropName) == companyId);
        }

    }
}
