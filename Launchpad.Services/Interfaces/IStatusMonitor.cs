using Launchpad.Models;
using Launchpad.Models.Enum;
using System.Collections.Generic;

namespace Launchpad.Services.Interfaces
{
    public interface IStatusMonitor
    {
        IEnumerable<StatusModel> GetStatus();
        string Name { get; }

        MonitorType Type {get;}
    }
}
