using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;

namespace Voyage.Data.Repositories.ClientRole
{
    public class ClientRoleRepository : BaseRepository<Models.Entities.ClientRole>, IClientRoleRepository
    {
        public ClientRoleRepository(IVoyageDataContext context)
            : base(context)
        {
        }

        public async override Task<Models.Entities.ClientRole> AddAsync(Models.Entities.ClientRole model)
        {
            Context.ClientRoles.Add(model);
            await Context.SaveChangesAsync();
            return model;
        }

        public async override Task<int> DeleteAsync(object id)
        {
            var entity = await GetAsync(id);
            if (entity == null)
                return 0;

            Context.ClientRoles.Remove(entity);
            return await Context.SaveChangesAsync();
        }

        public async override Task<Models.Entities.ClientRole> GetAsync(object id)
        {
            if (Context.ClientRoles is DbSet<Models.Entities.ClientRole> dbSet)
            {
                return await dbSet.FindAsync(id);
            }

            return Context.ClientRoles.Find(id);
        }

        public override IQueryable<Models.Entities.ClientRole> GetAll()
        {
            return Context.ClientRoles;
        }

        public async override Task<Models.Entities.ClientRole> UpdateAsync(Models.Entities.ClientRole model)
        {
            Context.ClientRoles.AddOrUpdate(model);
            await Context.SaveChangesAsync();
            return model;
        }
    }
}
