using Launchpad.Data.Interfaces;
using Launchpad.Models.EntityFramework;
using System;
using System.Linq;

namespace Launchpad.Data
{
    /// <summary>
    /// Following our repository pattern - this is readonly (The logger will write the messages)
    /// </summary>
    public class ApplicationLogRepository : BaseRepository<ApplicationLog>, IApplicationLogRepository
    {
        public ApplicationLogRepository(ILaunchpadDataContext context) : base(context)
        {
        }

        public IQueryable<ApplicationLog> GetRecentActivity(int maxEvents = 10)
        {
            return Context.Logs.Take(maxEvents);
        }
    }
}
