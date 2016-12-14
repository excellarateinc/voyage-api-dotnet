using Launchpad.Data.Interfaces;
using Launchpad.Models.EntityFramework;
using System.Linq;

namespace Launchpad.Data
{
    public class RoleClaimRepository : BaseRepository<RoleClaim>, IRoleClaimRepository
    {
        public RoleClaimRepository(ILaunchpadDataContext context) : base(context)
        {
        }

        public RoleClaim GetByRoleAndClaim(string roleName, string claimType, string claimValue)
        {
            return Context.RoleClaims
                     .FirstOrDefault(_ => _.Role.Name == roleName && _.ClaimType == claimType && _.ClaimValue == claimValue);
        }

        public IQueryable<RoleClaim> GetClaimsByRole(string roleName)
        {
            return Context.RoleClaims
                     .Where(_ => _.Role.Name == roleName);
        }
    }
}
