using System.Linq;
using Launchpad.Data.Interfaces;

namespace Launchpad.Data.Repositories.ApplicationLog
{
    public interface IApplicationLogRepository : IRepository<Models.EntityFramework.ApplicationLog>
    {
        IQueryable<Models.EntityFramework.ApplicationLog> GetRecentActivity(int maxEvents = 10);
    }
}
