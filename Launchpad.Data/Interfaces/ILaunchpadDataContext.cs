using Launchpad.Models.EntityFramework;
using System.Data.Entity;

namespace Launchpad.Data.Interfaces
{
    public interface ILaunchpadDataContext
    {
        IDbSet<Widget> Widgets { get; set; }

        IDbSet<LaunchpadLog> Logs { get; set; }

        IDbSet<RoleClaim> RoleClaims { get; set; }

        int SaveChanges();
       
    }
}
