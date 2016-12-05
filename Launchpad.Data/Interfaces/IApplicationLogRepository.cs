using Launchpad.Models.EntityFramework;
using System.Linq;

namespace Launchpad.Data.Interfaces
{
    public interface IApplicationLogRepository : IRepository<ApplicationLog>
    {
        IQueryable<ApplicationLog> GetRecentActivity(int maxEvents = 10);
    }
}
