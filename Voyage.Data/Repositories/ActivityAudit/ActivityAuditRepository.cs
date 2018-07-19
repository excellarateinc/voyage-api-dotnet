using System;
using System.Data.Entity;
using System.Data.SqlClient;
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
            ((DbContext)Context).Database.ExecuteSqlCommand(
                "dbo.ActivityAuditInsert @RequestId, @Method, @Path, @IpAddress, @Date, @StatusCode, @Error, @UserName",
                new SqlParameter("@RequestId", model.RequestId ?? (object)DBNull.Value),
                new SqlParameter("@Method", model.Method ?? (object)DBNull.Value),
                new SqlParameter("@Path", model.Path ?? (object)DBNull.Value),
                new SqlParameter("@IpAddress", model.IpAddress ?? (object)DBNull.Value),
                new SqlParameter("@Date", model.Date),
                new SqlParameter("@StatusCode", model.StatusCode),
                new SqlParameter("@Error", model.Error ?? (object)DBNull.Value),
                new SqlParameter("@UserName", model.UserName ?? (object)DBNull.Value));

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
