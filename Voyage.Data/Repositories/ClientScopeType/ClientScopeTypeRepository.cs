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
    public class ClientScopeTypeRepository : BaseRepository<Models.Entities.ClientScopeType>, IClientScopeTypeRepository
    {
        public ClientScopeTypeRepository(IVoyageDataContext context)
            : base(context)
        {
        }

        public override ClientScopeType Add(ClientScopeType model)
        {
            Context.ClientScopeTypes.Add(model);
            Context.SaveChanges();
            return model;
        }

        public override void Delete(object id)
        {
            var entity = Context.ClientScopeTypes.Find(id);
            Context.ClientScopeTypes.Remove(entity);
        }

        public override ClientScopeType Get(object id)
        {
            return Context.ClientScopeTypes.Find(id);
        }

        public override IQueryable<ClientScopeType> GetAll()
        {
            return Context.ClientScopeTypes;
        }

        public List<string> GetScopeNamesByScopes(List<ClientScope> list)
        {
            var scopes = new List<string>();
            for (int i = 0; i < list.Count; i++)
            {
                var scopeType = Get(list[i].Id);
                if (scopeType != null && !string.IsNullOrEmpty(scopeType.Name))
                {
                    scopes.Add(scopeType.Name);
                }
            }

            return scopes;
        }

        public override ClientScopeType Update(ClientScopeType model)
        {
            Context.ClientScopeTypes.AddOrUpdate(model);
            Context.SaveChanges();
            return model;
        }
    }
}
