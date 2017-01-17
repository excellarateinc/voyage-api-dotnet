using System.Data.Entity;
using Launchpad.Models.Entities;

namespace Launchpad.Data
{
    public interface ILaunchpadDataContext
    {
        IDbSet<ApplicationLog> Logs { get; set; }

        IDbSet<RoleClaim> RoleClaims { get; set; }

        IDbSet<ApplicationUser> Users { get; set; }

        IDbSet<ActivityAudit> ActivityAudits { get; set; }

        IDbSet<UserPhone> UserPhones { get; set; }

        int SaveChanges();
    }
}
