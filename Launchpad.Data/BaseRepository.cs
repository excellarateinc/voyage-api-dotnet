using Launchpad.Core;
using Launchpad.Data.Interfaces;
using System.Linq;

namespace Launchpad.Data
{
    /// <summary>
    /// Abstract implementation of the repository interface
    /// </summary>
    /// <typeparam name="TModel">Generic TModel which the repository will work with</typeparam>
    public abstract class BaseRepository<TModel> : IRepository<TModel> where TModel : class
    {
        protected ILaunchpadDataContext Context;

        protected BaseRepository(ILaunchpadDataContext context)
        {
            Context = context.ThrowIfNull(nameof(context));
        }

        public virtual TModel Add(TModel model)
        {
            Context.Set<TModel>().Add(model);
            return model;
        }

        public virtual TModel Update(TModel model)
        {
            // TODO: Handle administrative columns
            return model;
        }

        public virtual IQueryable<TModel> GetAll()
        {
            return Context.Set<TModel>();
        }

        public virtual TModel Get(object id)
        {
            return Context.Set<TModel>().Find(id);
        }

        public virtual void Delete(object id)
        {
            var entity = Get(id);
            if (entity == null)
                return;
            Context.Set<TModel>().Remove(entity);
        }
    }
}
