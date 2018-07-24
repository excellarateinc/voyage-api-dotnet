using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;

namespace Voyage.Data.Repositories.RoleClaim
{
    public class RoleClaimRepository : BaseRepository<Models.Entities.RoleClaim>, IRoleClaimRepository
    {
        public RoleClaimRepository(IVoyageDataContext context)
            : base(context)
        {
        }

        public async override Task<Models.Entities.RoleClaim> AddAsync(Models.Entities.RoleClaim model)
        {
            Context.RoleClaims.Add(model);
            await Context.SaveChangesAsync();
            return model;
        }

        public async override Task<int> DeleteAsync(object id)
        {
            var entity = await GetAsync(id);
            if (entity == null)
                return 0;

            Context.RoleClaims.Remove(entity);
            return await Context.SaveChangesAsync();
        }

        public async override Task<Models.Entities.RoleClaim> GetAsync(object id)
        {
            if (Context.RoleClaims is DbSet<Models.Entities.RoleClaim> dbSet)
            {
                return await dbSet.FindAsync(id);
            }

            return Context.RoleClaims.Find(id);
        }

        public override IQueryable<Models.Entities.RoleClaim> GetAll()
        {
            return Context.RoleClaims;
        }

        public async Task<Models.Entities.RoleClaim> GetByRoleAndClaimAsync(string roleName, string claimType, string claimValue)
        {
            return await Context.RoleClaims
                     .FirstOrDefaultAsync(_ => _.Role.Name == roleName && _.ClaimType == claimType && _.ClaimValue == claimValue);
        }

        public IQueryable<Models.Entities.RoleClaim> GetClaimsByRole(string roleName)
        {
            return Context.RoleClaims
                     .Where(_ => _.Role.Name == roleName);
        }

        public async override Task<Models.Entities.RoleClaim> UpdateAsync(Models.Entities.RoleClaim model)
        {
            Context.RoleClaims.AddOrUpdate(model);
            await Context.SaveChangesAsync();
            return model;
        }
    }
}
