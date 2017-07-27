using System.Data.Entity.Migrations;
using System.Linq;

namespace Voyage.Data.Repositories.Banking
{
    public class AccountsRepository : BaseRepository<Models.Entities.Banking.Account>, IAccountsRepository
    {
        public AccountsRepository(IVoyageDataContext context)
            : base(context)
        {
        }

        public override Models.Entities.Banking.Account Add(Models.Entities.Banking.Account model)
        {
            Context.Accounts.Add(model);
            Context.SaveChanges();
            return model;
        }

        public override void Delete(object id)
        {
            var entity = Get(id);
            if (entity == null)
                return;

            Context.Accounts.Remove(entity);
            Context.SaveChanges();
        }

        public override Models.Entities.Banking.Account Get(object id)
        {
            return Context.Accounts.Find(id);
        }

        public override IQueryable<Models.Entities.Banking.Account> GetAll()
        {
            return Context.Accounts;
        }

        public override Models.Entities.Banking.Account Update(Models.Entities.Banking.Account model)
        {
            Context.Accounts.AddOrUpdate(model);
            Context.SaveChanges();
            return model;
        }
    }
}
