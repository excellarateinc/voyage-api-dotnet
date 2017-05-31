using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voyage.Models;
using Voyage.Models.Entities;

namespace Voyage.Data.Repositories.Client
{
    public class ClientScopeRepository : BaseRepository<Models.Entities.ClientScope>, IClientScopeRepository
    {
        public ClientScopeRepository(IVoyageDataContext context)
            : base(context)
        {
        }

        public override ClientScope Add(ClientScope model)
        {
            Context.ClientScopes.Add(model);
            Context.SaveChanges();
            return model;
        }

        public override void Delete(object id)
        {
            var entity = Get(id);
            if (entity != null)
            {
                Context.ClientScopes.Remove(entity);
                Context.SaveChanges();
            }
        }

        public override ClientScope Get(object id)
        {
            return Context.ClientScopes.Find(id);
        }

        public override IQueryable<ClientScope> GetAll()
        {
            return Context.ClientScopes;
        }

        public List<ClientScope> GetClientScopesByClientId(string id)
        {
            return Context.ClientScopes.Where(k => k.ClientId.Equals(id)).ToList();
        }

        public override ClientScope Update(ClientScope model)
        {
            Context.ClientScopes.AddOrUpdate(model);
            Context.SaveChanges();
            return model;
        }
    }
}
