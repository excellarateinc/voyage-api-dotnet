using TrackerEnabledDbContext.Common.Configuration;

namespace Launchpad.Data.Auditing
{
    public abstract class BaseAuditConfiguration<TModel> : IAuditConfiguration
    {
        public virtual void Configure()
        {
            EntityTracker
                .TrackAllProperties<TModel>();
        }
    }
}
