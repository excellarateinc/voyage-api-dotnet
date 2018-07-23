using System;
using System.Linq;
using System.Threading.Tasks;

namespace Voyage.Data.Repositories.ApplicationLog
{
    /// <summary>
    /// Following our repository pattern - this is readonly (The logger will write the messages)
    /// </summary>
    public class ApplicationLogRepository : BaseRepository<Models.Entities.ApplicationLog>, IApplicationLogRepository
    {
        public ApplicationLogRepository(IVoyageDataContext context)
            : base(context)
        {
        }

        public override Models.Entities.ApplicationLog Add(Models.Entities.ApplicationLog model)
        {
            throw new NotImplementedException("Log messages managed by logging interface");
        }

        public override Task<Models.Entities.ApplicationLog> AddAsync(Models.Entities.ApplicationLog model)
        {
            throw new NotImplementedException("Log messages managed by logging interface");
        }

        public override int Delete(object id)
        {
            throw new NotImplementedException("Log messages managed by logging interface");
        }

        public override Task<int> DeleteAsync(object id)
        {
            throw new NotImplementedException("Log messages managed by logging interface");
        }

        public override Models.Entities.ApplicationLog Get(object id)
        {
            throw new NotImplementedException("No current use case for retrieving a message by id");
        }

        public override Task<Models.Entities.ApplicationLog> GetAsync(object id)
        {
            throw new NotImplementedException("No current use case for retrieving a message by id");
        }

        public override IQueryable<Models.Entities.ApplicationLog> GetAll()
        {
            throw new NotImplementedException("No current user case of retrieving all messages");
        }

        public IQueryable<Models.Entities.ApplicationLog> GetRecentActivity(int maxEvents = 10)
        {
            return Context.Logs.Take(maxEvents);
        }

        public override Models.Entities.ApplicationLog Update(Models.Entities.ApplicationLog model)
        {
            throw new NotImplementedException("Log messages managed by logging interface");
        }

        public override Task<Models.Entities.ApplicationLog> UpdateAsync(Models.Entities.ApplicationLog model)
        {
            throw new NotImplementedException("Log messages managed by logging interface");
        }
    }
}
