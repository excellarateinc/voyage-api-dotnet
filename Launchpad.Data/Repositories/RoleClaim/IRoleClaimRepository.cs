using System.Linq;
using Launchpad.Data.Interfaces;

namespace Launchpad.Data.Repositories.RoleClaim
{
    public interface IRoleClaimRepository : IRepository<Models.EntityFramework.RoleClaim>
    {
        IQueryable<Models.EntityFramework.RoleClaim> GetClaimsByRole(string roleName);

        Models.EntityFramework.RoleClaim GetByRoleAndClaim(string roleName, string claimType, string claimValue);
    }
}
