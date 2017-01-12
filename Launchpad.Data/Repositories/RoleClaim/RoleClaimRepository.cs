using System.Linq;

namespace Launchpad.Data.Repositories.RoleClaim
{
    public class RoleClaimRepository : BaseRepository<Models.Entities.RoleClaim>, IRoleClaimRepository
    {
        public RoleClaimRepository(ILaunchpadDataContext context) : base(context)
        {
        }

        public override Models.Entities.RoleClaim Add(Models.Entities.RoleClaim model)
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

        public override Models.Entities.RoleClaim Get(object id)
        {
            return Context.RoleClaims.Find(id);
        }

        public override IQueryable<Models.Entities.RoleClaim> GetAll()
        {
            return Context.RoleClaims;
        }

        public Models.Entities.RoleClaim GetByRoleAndClaim(string roleName, string claimType, string claimValue)
        {
            return Context.RoleClaims
                     .FirstOrDefault(_ => _.Role.Name == roleName && _.ClaimType == claimType && _.ClaimValue == claimValue);
        }

        public IQueryable<Models.Entities.RoleClaim> GetClaimsByRole(string roleName)
        {
            return Context.RoleClaims
                     .Where(_ => _.Role.Name == roleName);
        }

        public override Models.Entities.RoleClaim Update(Models.Entities.RoleClaim model)
        {
            Context.SaveChanges();
            return model;
        }
    }
}
