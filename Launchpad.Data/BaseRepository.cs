using System.Linq;
using Launchpad.Data.Interfaces;
using Launchpad.Core;

namespace Launchpad.Data
{
    public abstract class BaseRepository<TModel> : IRepository<TModel>
    {
        protected ILaunchpadDataContext Context;

        public BaseRepository(ILaunchpadDataContext context)
        {
            Context = context.ThrowIfNull(nameof(context));
        }

        public abstract IQueryable<TModel> GetAll();

        public abstract TModel Get(object id);

        public int SaveChanges()
        {
            return Context.SaveChanges();
        }
    }
}
