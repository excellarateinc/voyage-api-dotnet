using System.Linq;
using System.Threading.Tasks;

namespace Voyage.Data.Repositories
{
    /// <summary>
    /// Basic repository
    /// </summary>
    public interface IRepository
    {
        Task<int> SaveChangesAsync();
    }

    /// <summary>
    /// Repository that returns generic type TModel
    /// </summary>
    /// <typeparam name="TModel">Type of the model that the repository will return</typeparam>
    public interface IRepository<TModel> : IRepository
    {
        Task<TModel> AddAsync(TModel model);

        Task<TModel> UpdateAsync(TModel model);

        IQueryable<TModel> GetAll();

        Task<TModel> GetAsync(object id);

        Task<int> DeleteAsync(object id);
    }
}
