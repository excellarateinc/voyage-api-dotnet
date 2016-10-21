using Launchpad.Data.Interfaces;
using Launchpad.Models.EntityFramework;
using System;
using System.Linq;

namespace Launchpad.Data
{
    /// <summary>
    /// Following our repository pattern - this is readonly (The logger will write the messages)
    /// </summary>
    public class LaunchpadLogRepository : BaseRepository<LaunchpadLog>, ILaunchpadLogRepository
    {
        public LaunchpadLogRepository(ILaunchpadDataContext context) : base(context)
        {
        }

        public override LaunchpadLog Add(LaunchpadLog model)
        {
            throw new NotImplementedException("Log messages managed by logging interface");
        }

        public override void Delete(object id)
        {
            throw new NotImplementedException("Log messages managed by logging interface");
        }

        public override LaunchpadLog Get(object id)
        {
            throw new NotImplementedException("No current use case for retrieving a message by id");
        }

        public override IQueryable<LaunchpadLog> GetAll()
        {
            throw new NotImplementedException("No current user case of retrieving all messages");
        }

        public IQueryable<LaunchpadLog> GetRecentActivity(int maxEvents = 10)
        {
            return Context.Logs.Take(maxEvents);
        }

        public override LaunchpadLog Update(LaunchpadLog model)
        {
            throw new NotImplementedException("Log messages managed by logging interface");
        }
    }
}
