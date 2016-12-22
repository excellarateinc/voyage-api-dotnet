using Launchpad.Data.Interfaces;
using System.Data.Entity;

namespace Launchpad.Data
{
    public class TransactionWrapper : ITransaction
    {
        private readonly DbContextTransaction _transaction;

        public TransactionWrapper(DbContextTransaction transaction)
        {
            _transaction = transaction;
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Dispose()
        {
            _transaction.Dispose();
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }
    }
}
