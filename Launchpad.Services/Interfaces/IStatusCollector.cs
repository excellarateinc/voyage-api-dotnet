using Launchpad.Models;
using Launchpad.Models.Enum;
using System.Collections.Generic;

namespace Launchpad.Services.Interfaces
{
    public interface IStatusCollector
    {
        IEnumerable<StatusAggregateModel> Collect();

        IEnumerable<StatusAggregateModel> Collect(MonitorType type);
    }
}
