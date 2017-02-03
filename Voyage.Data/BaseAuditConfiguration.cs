using Voyage.Models.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using TrackerEnabledDbContext.Common.Configuration;

namespace Voyage.Data
{
    public static class BaseAuditConfiguration
    {
        public static void Configure()
        {
            EntityTracker.TrackAllProperties<ApplicationRole>();

            EntityTracker.TrackAllProperties<ApplicationUser>()
                .Except(_ => _.PasswordHash);

            EntityTracker.TrackAllProperties<IdentityUserClaim>();

            EntityTracker.TrackAllProperties<IdentityUserLogin>();

            EntityTracker.TrackAllProperties<IdentityUserRole>();

            EntityTracker.TrackAllProperties<RoleClaim>();

            EntityTracker.TrackAllProperties<UserPhone>();
        }
    }
}
