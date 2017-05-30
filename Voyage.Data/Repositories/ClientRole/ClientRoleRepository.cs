using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voyage.Models;
using Voyage.Models.Entities;
using System.Data.Entity.Migrations;

namespace Voyage.Data.Repositories.Client
{
    public class ClientRoleRepository : BaseRepository<Models.Entities.ClientRole>, IClientRoleRepository
    {
        public ClientRoleRepository(IVoyageDataContext context)
            : base(context)
        {
        }

        public override ClientRole Add(ClientRole model)
        {
            Context.ClientRoles.Add(model);
            Context.SaveChanges();
            return model;
        }

        public override void Delete(object id)
        {
            var entity = Context.ClientRoles.Find(id);
            Context.ClientRoles.Remove(entity);
        }

        public override ClientRole Get(object id)
        {
            return Context.ClientRoles.Find(id);
        }

        public override IQueryable<ClientRole> GetAll()
        {
            return Context.ClientRoles;
        }

        public override ClientRole Update(ClientRole model)
        {
            Context.ClientRoles.AddOrUpdate(model);
            Context.SaveChanges();
            return model;
        }
    }
}
