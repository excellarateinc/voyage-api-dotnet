using Launchpad.Models.EntityFramework;
using System.Data.Entity;

namespace Launchpad.Data.Interfaces
{
    public interface ILaunchpadDataContext
    {
        IDbSet<Widget> Widgets { get; set; }

        int SaveChanges();
       
    }
}
