using System.Linq;
using Launchpad.Data.Interfaces;

namespace Launchpad.Data.Repositories.RoleClaim
{
    public class RoleClaimRepository : BaseRepository<Models.EntityFramework.RoleClaim>, IRoleClaimRepository
    {
        public RoleClaimRepository(ILaunchpadDataContext context) : base(context)
        {
        }

        public override Models.EntityFramework.RoleClaim Add(Models.EntityFramework.RoleClaim model)
        {
            Context.RoleClaims.Add(model);
            Context.SaveChanges();
            return model;
        }

        public override void Delete(object id)
        {
            var entity = Get(id);
            if (entity != null)
            {
                Context.RoleClaims.Remove(entity);
                Context.SaveChanges();
            }
        }

        public override Models.EntityFramework.RoleClaim Get(object id)
        {
            return Context.RoleClaims.Find(id);
        }

        public override IQueryable<Models.EntityFramework.RoleClaim> GetAll()
        {
            return Context.RoleClaims;
        }

        public Models.EntityFramework.RoleClaim GetByRoleAndClaim(string roleName, string claimType, string claimValue)
        {
            return Context.RoleClaims
                     .FirstOrDefault(_ => _.Role.Name == roleName && _.ClaimType == claimType && _.ClaimValue == claimValue);
        }

        public IQueryable<Models.EntityFramework.RoleClaim> GetClaimsByRole(string roleName)
        {
            return Context.RoleClaims
                     .Where(_ => _.Role.Name == roleName);
        }

        public override Models.EntityFramework.RoleClaim Update(Models.EntityFramework.RoleClaim model)
        {
            Context.SaveChanges();
            return model;
        }
    }
}
