using System.Data.Entity.Migrations;
using System.Linq;

namespace Voyage.Data.Repositories.ClientRole
{
    public class ClientRoleRepository : BaseRepository<Models.Entities.ClientRole>, IClientRoleRepository
    {
        public ClientRoleRepository(IVoyageDataContext context)
            : base(context)
        {
        }

        public override Models.Entities.ClientRole Add(Models.Entities.ClientRole model)
        {
            Context.ClientRoles.Add(model);
            Context.SaveChanges();
            return model;
        }

        public override void Delete(object id)
        {
            var entity = Get(id);
            if (entity == null)
                return;

            Context.ClientRoles.Remove(entity);
            Context.SaveChanges();
        }

        public override Models.Entities.ClientRole Get(object id)
        {
            return Context.ClientRoles.Find(id);
        }

        public override IQueryable<Models.Entities.ClientRole> GetAll()
        {
            return Context.ClientRoles;
        }

        public override Models.Entities.ClientRole Update(Models.Entities.ClientRole model)
        {
            Context.ClientRoles.AddOrUpdate(model);
            Context.SaveChanges();
            return model;
        }
    }
}
