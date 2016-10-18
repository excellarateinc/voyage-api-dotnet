using System.Linq;

namespace Launchpad.Data.Interfaces
{
    public interface IRepository
    {
        int SaveChanges();
    }

    public interface IRepository<TModel> : IRepository
    {
        IQueryable<TModel> GetAll();
        TModel Get(object key);
    
    }
}
