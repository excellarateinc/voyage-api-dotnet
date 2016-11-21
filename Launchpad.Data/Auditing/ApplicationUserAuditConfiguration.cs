using Launchpad.Models.EntityFramework;
using TrackerEnabledDbContext.Common.Configuration;

namespace Launchpad.Data.Auditing
{
    public class ApplicationUserAuditConfiguration : BaseAuditConfiguration<ApplicationUser>
    {
        public override void Configure()
        {
            EntityTracker
                .TrackAllProperties<ApplicationUser>()
                .Except(_ => _.PasswordHash);
        }
    }
}
