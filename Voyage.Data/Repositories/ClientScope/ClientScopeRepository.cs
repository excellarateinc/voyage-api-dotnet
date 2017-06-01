using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace Voyage.Data.Repositories.ClientScope
{
    public class ClientScopeRepository : BaseRepository<Models.Entities.ClientScope>, IClientScopeRepository
    {
        public ClientScopeRepository(IVoyageDataContext context)
            : base(context)
        {
        }

        public override Models.Entities.ClientScope Add(Models.Entities.ClientScope model)
        {
            Context.ClientScopes.Add(model);
            Context.SaveChanges();
            return model;
        }

        public override void Delete(object id)
        {
            var entity = Get(id);
            if (entity == null)
                return;

            Context.ClientScopes.Remove(entity);
            Context.SaveChanges();
        }

        public override Models.Entities.ClientScope Get(object id)
        {
            return Context.ClientScopes.Find(id);
        }

        public override IQueryable<Models.Entities.ClientScope> GetAll()
        {
            return Context.ClientScopes;
        }

        public override Models.Entities.ClientScope Update(Models.Entities.ClientScope model)
        {
            Context.ClientScopes.AddOrUpdate(model);
            Context.SaveChanges();
            return model;
        }
    }
}
