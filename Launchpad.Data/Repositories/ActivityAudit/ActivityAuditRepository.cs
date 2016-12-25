using System;
using System.Linq;
using Launchpad.Data.Interfaces;

namespace Launchpad.Data.Repositories.ActivityAudit
{
    public class ActivityAuditRepository : BaseRepository<Models.EntityFramework.ActivityAudit>, IActivityAuditRepository
    {
        public ActivityAuditRepository(ILaunchpadDataContext context) : base(context)
        {
        }

        public override Models.EntityFramework.ActivityAudit Add(Models.EntityFramework.ActivityAudit model)
        {
            Context.ActivityAudits.Add(model);
            Context.SaveChanges();
            return model;
        }

        public override void Delete(object id)
        {
            throw new NotImplementedException("No requirement for deleting an activity audit record");
        }

        public override Models.EntityFramework.ActivityAudit Get(object id)
        {
            return Context.ActivityAudits.Find(id);
        }

        public override IQueryable<Models.EntityFramework.ActivityAudit> GetAll()
        {
            return Context.ActivityAudits;
        }

        public override Models.EntityFramework.ActivityAudit Update(Models.EntityFramework.ActivityAudit model)
        {
            throw new NotImplementedException("No requirement for updating an activity audit record");
        }
    }
}
