using Launchpad.Models;
using System.Data.Entity;

namespace Launchpad.Data
{
    public interface ILaunchpadDataContext
    {
        IDbSet<Widget> Widgets { get; set; }
    }
}
