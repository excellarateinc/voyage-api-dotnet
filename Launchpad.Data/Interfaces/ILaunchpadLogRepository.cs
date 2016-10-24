using Launchpad.Models.EntityFramework;
using System.Linq;

namespace Launchpad.Data.Interfaces
{
    public interface ILaunchpadLogRepository : IRepository<LaunchpadLog>
    {
        IQueryable<LaunchpadLog> GetRecentActivity(int maxEvents = 10);
    }
}
