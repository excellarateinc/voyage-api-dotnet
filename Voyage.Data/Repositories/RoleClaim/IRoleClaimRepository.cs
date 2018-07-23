using System.Linq;
using System.Threading.Tasks;

namespace Voyage.Data.Repositories.RoleClaim
{
    public interface IRoleClaimRepository : IRepository<Models.Entities.RoleClaim>
    {
        IQueryable<Models.Entities.RoleClaim> GetClaimsByRole(string roleName);

        Task<Models.Entities.RoleClaim> GetByRoleAndClaimAsync(string roleName, string claimType, string claimValue);
    }
}
