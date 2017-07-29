using System.Data.Entity.Migrations;
using System.Linq;

namespace Voyage.Data.Repositories.Banking
{
    public class TransactionsRepository : BaseRepository<Models.Entities.Banking.Transaction>, ITransactionsRepository
    {
        public TransactionsRepository(IVoyageDataContext context)
            : base(context)
        {
        }

        public override Models.Entities.Banking.Transaction Add(Models.Entities.Banking.Transaction model)
        {
            Context.Transactions.Add(model);
            Context.SaveChanges();
            return model;
        }

        public override void Delete(object id)
        {
            var entity = Get(id);
            if (entity == null)
                return;

            Context.Transactions.Remove(entity);
            Context.SaveChanges();
        }

        public override Models.Entities.Banking.Transaction Get(object id)
        {
            return Context.Transactions.Find(id);
        }

        public override IQueryable<Models.Entities.Banking.Transaction> GetAll()
        {
            return Context.Transactions;
        }

        public override Models.Entities.Banking.Transaction Update(Models.Entities.Banking.Transaction model)
        {
            Context.Transactions.AddOrUpdate(model);
            Context.SaveChanges();
            return model;
        }
    }
}
