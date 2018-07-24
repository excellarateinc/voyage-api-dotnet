using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Voyage.Data.Repositories.ActivityAudit
{
    public class ActivityAuditRepository : BaseRepository<Models.Entities.ActivityAudit>, IActivityAuditRepository
    {
        public ActivityAuditRepository(IVoyageDataContext context)
            : base(context)
        {
        }

        public async override Task<Models.Entities.ActivityAudit> AddAsync(Models.Entities.ActivityAudit model)
        {
            await ((DbContext)Context).Database.ExecuteSqlCommandAsync(
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

        public override Task<int> DeleteAsync(object id)
        {
            throw new NotImplementedException("No requirement for deleting an activity audit record");
        }

        public async override Task<Models.Entities.ActivityAudit> GetAsync(object id)
        {
            if (Context.ActivityAudits is DbSet<Models.Entities.ActivityAudit> dbSet)
            {
                return await dbSet.FindAsync(id);
            }

            return Context.ActivityAudits.Find(id);
        }

        public override IQueryable<Models.Entities.ActivityAudit> GetAll()
        {
            return Context.ActivityAudits;
        }

        public override Task<Models.Entities.ActivityAudit> UpdateAsync(Models.Entities.ActivityAudit model)
        {
            throw new NotImplementedException("No requirement for updating an activity audit record");
        }
    }
}
