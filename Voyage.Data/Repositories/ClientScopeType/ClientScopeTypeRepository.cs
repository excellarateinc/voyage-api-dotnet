using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;

namespace Voyage.Data.Repositories.ClientScopeType
{
    public class ClientScopeTypeRepository : BaseRepository<Models.Entities.ClientScopeType>, IClientScopeTypeRepository
    {
        public ClientScopeTypeRepository(IVoyageDataContext context)
            : base(context)
        {
        }

        public override Models.Entities.ClientScopeType Add(Models.Entities.ClientScopeType model)
        {
            Context.ClientScopeTypes.Add(model);
            Context.SaveChanges();
            return model;
        }

        public async override Task<Models.Entities.ClientScopeType> AddAsync(Models.Entities.ClientScopeType model)
        {
            Context.ClientScopeTypes.Add(model);
            await Context.SaveChangesAsync();
            return model;
        }

        public override int Delete(object id)
        {
            var entity = Get(id);
            if (entity == null)
                return 0;

            Context.ClientScopeTypes.Remove(entity);
            return Context.SaveChanges();
        }

        public async override Task<int> DeleteAsync(object id)
        {
            var entity = await GetAsync(id);
            if (entity == null)
                return 0;

            Context.ClientScopeTypes.Remove(entity);
            return await Context.SaveChangesAsync();
        }

        public override Models.Entities.ClientScopeType Get(object id)
        {
            return Context.ClientScopeTypes.Find(id);
        }

        public async override Task<Models.Entities.ClientScopeType> GetAsync(object id)
        {
            if (Context.ClientScopeTypes is DbSet<Models.Entities.ClientScopeType> dbSet)
            {
                return await dbSet.FindAsync(id);
            }

            return Context.ClientScopeTypes.Find(id);
        }

        public override IQueryable<Models.Entities.ClientScopeType> GetAll()
        {
            return Context.ClientScopeTypes;
        }

        public override Models.Entities.ClientScopeType Update(Models.Entities.ClientScopeType model)
        {
            Context.ClientScopeTypes.AddOrUpdate(model);
            Context.SaveChanges();
            return model;
        }

        public async override Task<Models.Entities.ClientScopeType> UpdateAsync(Models.Entities.ClientScopeType model)
        {
            Context.ClientScopeTypes.AddOrUpdate(model);
            await Context.SaveChangesAsync();
            return model;
        }
    }
}
