using System.Threading.Tasks;
using Launchpad.Models;

namespace Launchpad.Services.Audit
{
    public interface IAuditService
    {
        void Record(ActivityAuditModel model);

        Task RecordAsync(ActivityAuditModel model);
    }
}
