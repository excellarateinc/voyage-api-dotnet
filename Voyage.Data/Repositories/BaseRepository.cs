using System.Linq;
using System.Threading.Tasks;
using Voyage.Core;

namespace Voyage.Data.Repositories
{
    /// <summary>
    /// Abstract implementation of the repository interface
    /// </summary>
    /// <typeparam name="TModel">Generic TModel which the repository will work with</typeparam>
    public abstract class BaseRepository<TModel> : IRepository<TModel>
    {
#pragma warning disable SA1401 // Fields must be private
#pragma warning disable SA1306 // Field names must begin with lower-case letter
        protected IVoyageDataContext Context;
#pragma warning restore SA1306 // Field names must begin with lower-case letter
#pragma warning restore SA1401 // Fields must be private

        protected BaseRepository(IVoyageDataContext context)
        {
            Context = context.ThrowIfNull(nameof(context));
        }

        public abstract TModel Add(TModel model);

        public abstract Task<TModel> AddAsync(TModel model);

        public abstract TModel Update(TModel model);

        public abstract Task<TModel> UpdateAsync(TModel model);

        public abstract IQueryable<TModel> GetAll();

        public abstract TModel Get(object id);

        public abstract Task<TModel> GetAsync(object id);

        public abstract int Delete(object id);

        public abstract Task<int> DeleteAsync(object id);

        public int SaveChanges()
        {
            return Context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await Context.SaveChangesAsync();
        }
    }
}
