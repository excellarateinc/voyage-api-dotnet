using System;
using System.Linq;

namespace Voyage.Data.Repositories.ActivityAudit
{
    public class ActivityAuditRepository : BaseRepository<Models.Entities.ActivityAudit>, IActivityAuditRepository
    {
        public ActivityAuditRepository(IVoyageDataContext context)
            : base(context)
        {
        }

        public override Models.Entities.ActivityAudit Add(Models.Entities.ActivityAudit model)
        {
            Context.ActivityAudits.Add(model);
            Context.SaveChanges();
            return model;
        }

        public override void Delete(object id)
        {
            throw new NotImplementedException("No requirement for deleting an activity audit record");
        }

        public override Models.Entities.ActivityAudit Get(object id)
        {
            return Context.ActivityAudits.Find(id);
        }

        public override IQueryable<Models.Entities.ActivityAudit> GetAll()
        {
            return Context.ActivityAudits;
        }

        public override Models.Entities.ActivityAudit Update(Models.Entities.ActivityAudit model)
        {
            throw new NotImplementedException("No requirement for updating an activity audit record");
        }
    }
}
