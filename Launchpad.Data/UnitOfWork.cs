using Launchpad.Core;
using Launchpad.Data.Interfaces;

namespace Launchpad.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IPersistanceComponent _component;

        public UnitOfWork(IPersistanceComponent component)
        {
            _component = component.ThrowIfNull(nameof(component));
        }

        public ITransaction Begin()
        {
            return _component.BeginTransaction();
        }

        public int SaveChanges()
        {
            return _component.SaveChanges();
        }
    }
}
