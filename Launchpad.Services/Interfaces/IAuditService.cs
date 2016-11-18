using Launchpad.Models;
using System.Threading.Tasks;

namespace Launchpad.Services.Interfaces
{
    public interface IAuditService
    {
        void Record(ActivityAuditModel model);
        Task RecordAsync(ActivityAuditModel model);
    }
}
