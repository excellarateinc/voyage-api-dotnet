using System.Data.Entity.Migrations;
using System.Linq;

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

        public override void Delete(object id)
        {
            var entity = Get(id);
            if (entity == null)
                return;

            Context.ClientScopeTypes.Remove(entity);
            Context.SaveChanges();
        }

        public override Models.Entities.ClientScopeType Get(object id)
        {
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
    }
}
