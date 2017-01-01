using System.Linq;
using Launchpad.Core;

namespace Launchpad.Data.Repositories
{
    /// <summary>
    /// Abstract implementation of the repository interface
    /// </summary>
    /// <typeparam name="TModel">Generic TModel which the repository will work with</typeparam>
    public abstract class BaseRepository<TModel> : IRepository<TModel>
    {
        protected ILaunchpadDataContext Context;

        protected BaseRepository(ILaunchpadDataContext context)
        {
            Context = context.ThrowIfNull(nameof(context));
        }

        public abstract TModel Add(TModel model);

        public abstract TModel Update(TModel model);

        public abstract IQueryable<TModel> GetAll();

        public abstract TModel Get(object id);

        public abstract void Delete(object id);

        public int SaveChanges()
        {
            return Context.SaveChanges();
        }
    }
}
