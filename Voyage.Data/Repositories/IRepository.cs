using System.Linq;

namespace Voyage.Data.Repositories
{
    /// <summary>
    /// Basic repository
    /// </summary>
    public interface IRepository
    {
        int SaveChanges();
    }

    /// <summary>
    /// Repository that returns generic type TModel
    /// </summary>
    /// <typeparam name="TModel">Type of the model that the repository will return</typeparam>
    public interface IRepository<TModel> : IRepository
    {
        IQueryable<TModel> GetAll();

        TModel Get(object key);

        TModel Add(TModel model);

        TModel Update(TModel model);

        void Delete(object key);
    }
}
