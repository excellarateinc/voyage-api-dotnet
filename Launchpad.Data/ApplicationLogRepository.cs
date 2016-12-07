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

        public override ApplicationLog Add(ApplicationLog model)
        {
            throw new NotImplementedException("Log messages managed by logging interface");
        }

        public override void Delete(object id)
        {
            throw new NotImplementedException("Log messages managed by logging interface");
        }

        public override ApplicationLog Get(object id)
        {
            throw new NotImplementedException("No current use case for retrieving a message by id");
        }

        public override IQueryable<ApplicationLog> GetAll()
        {
            throw new NotImplementedException("No current user case of retrieving all messages");
        }

        public IQueryable<ApplicationLog> GetRecentActivity(int maxEvents = 10)
        {
            return Context.Logs.Take(maxEvents);
        }

        public override ApplicationLog Update(ApplicationLog model)
        {
            throw new NotImplementedException("Log messages managed by logging interface");
        }
    }
}
