using System;
using System.Linq;
using Launchpad.Data.Interfaces;
using Launchpad.Models.EntityFramework;

namespace Launchpad.Data
{
    public class ActivityAuditRepository : BaseRepository<ActivityAudit>, IActivityAuditRepository
    {
        public ActivityAuditRepository(ILaunchpadDataContext context) : base(context)
        {
        }

        public override ActivityAudit Add(ActivityAudit model)
        {
            Context.ActivityAudits.Add(model);
            Context.SaveChanges();
            return model;
        }

        public override void Delete(object id)
        {
            throw new NotImplementedException("No requirement for deleting an activity audit record");
        }

        public override ActivityAudit Get(object id)
        {
            return Context.ActivityAudits.Find(id);
        }

        public override IQueryable<ActivityAudit> GetAll()
        {
            return Context.ActivityAudits;
        }

        public override ActivityAudit Update(ActivityAudit model)
        {
            throw new NotImplementedException("No requirement for updating an activity audit record");
        }
    }
}
