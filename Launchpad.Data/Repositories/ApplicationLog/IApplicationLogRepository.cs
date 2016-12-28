using System.Linq;
using Launchpad.Data.Interfaces;

namespace Launchpad.Data.Repositories.ApplicationLog
{
    public interface IApplicationLogRepository : IRepository<Models.Entities.ApplicationLog>
    {
        IQueryable<Models.Entities.ApplicationLog> GetRecentActivity(int maxEvents = 10);
    }
}
