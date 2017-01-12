using System.Linq;

namespace Launchpad.Data.Repositories.RoleClaim
{
    public interface IRoleClaimRepository : IRepository<Models.Entities.RoleClaim>
    {
        IQueryable<Models.Entities.RoleClaim> GetClaimsByRole(string roleName);

        Models.Entities.RoleClaim GetByRoleAndClaim(string roleName, string claimType, string claimValue);
    }
}
