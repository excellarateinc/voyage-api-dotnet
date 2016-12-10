using System.Linq;
using Launchpad.Models.EntityFramework;

namespace Launchpad.Data.Interfaces
{
    public interface IApplicationLogRepository : IRepository<ApplicationLog>
    {
        IQueryable<ApplicationLog> GetRecentActivity(int maxEvents = 10);
    }
}
