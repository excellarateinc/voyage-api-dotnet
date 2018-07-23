using System.Linq;
using System.Threading.Tasks;

namespace Voyage.Data.Repositories
{
    /// <summary>
    /// Basic repository
    /// </summary>
    public interface IRepository
    {
        // int SaveChanges();
        Task<int> SaveChangesAsync();
    }

    /// <summary>
    /// Repository that returns generic type TModel
    /// </summary>
    /// <typeparam name="TModel">Type of the model that the repository will return</typeparam>
    public interface IRepository<TModel> : IRepository
    {
        // TModel Add(TModel model);
        Task<TModel> AddAsync(TModel model);

        // TModel Update(TModel model);
        Task<TModel> UpdateAsync(TModel model);

        IQueryable<TModel> GetAll();

        // Model Get(object id);
        Task<TModel> GetAsync(object id);

        // int Delete(object id);
        Task<int> DeleteAsync(object id);
    }
}
