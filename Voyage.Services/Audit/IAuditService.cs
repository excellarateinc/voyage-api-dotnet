using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Voyage.Models;
using Voyage.Models.Entities;

namespace Voyage.Services.Audit
{
    public interface IAuditService
    {
        void Record(ActivityAuditModel model);

        Task RecordAsync(ActivityAuditModel model);

        IQueryable<ActivityAudit> GeAudittActivityWithinTime(string userName, string path, int timeInMinutes);
    }
}
