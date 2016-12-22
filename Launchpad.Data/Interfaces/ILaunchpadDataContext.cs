using Launchpad.Models.EntityFramework;
using System.Data.Entity;

namespace Launchpad.Data.Interfaces
{
    public interface ILaunchpadDataContext
    {
        IDbSet<Widget> Widgets { get; set; }

        IDbSet<ApplicationLog> Logs { get; set; }

        IDbSet<RoleClaim> RoleClaims { get; set; }

        IDbSet<ApplicationUser> Users { get; set; }

        IDbSet<ActivityAudit> ActivityAudits { get; set; }

        IDbSet<UserPhone> UserPhones { get; set; }

        IDbSet<TEntity> Set<TEntity>() where TEntity : class;
    }
}
