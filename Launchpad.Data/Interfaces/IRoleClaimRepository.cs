using Launchpad.Models.EntityFramework;
using System.Linq;

namespace Launchpad.Data.Interfaces
{
    public interface IRoleClaimRepository : IRepository<RoleClaim>
    {
        IQueryable<RoleClaim> GetClaimsByRole(string roleName);

        RoleClaim GetByRoleAndClaim(string roleName, string claimType, string claimValue);
    }
}
