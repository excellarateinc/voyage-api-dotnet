using Launchpad.Models.EntityFramework;
using System.Data.Entity;

namespace Launchpad.Data.Interfaces
{
    public interface ILaunchpadDataContext
    {
        IDbSet<Widget> Widgets { get; set; }

        IDbSet<LaunchpadLog> Logs { get; set; }

        IDbSet<RoleClaim> RoleClaims { get; set; }

        IDbSet<ApplicationUser> Users { get; set; }

        IDbSet<ActivityAudit> ActivityAudits { get; set; }

        IDbSet<UserPhone> UserPhones { get; set; }

        int SaveChanges();
       
    }
}
