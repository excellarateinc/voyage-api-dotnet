using System.Threading.Tasks;
using Voyage.Models;

namespace Voyage.Services.Audit
{
    public interface IAuditService
    {
        void Record(ActivityAuditModel model);

        Task RecordAsync(ActivityAuditModel model);
    }
}
