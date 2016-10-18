using Launchpad.Models;
using System.Data.Entity;

namespace Launchpad.Data.Interfaces
{
    public interface ILaunchpadDataContext
    {
        IDbSet<Widget> Widgets { get; set; }

        int SaveChanges();
    }
}
