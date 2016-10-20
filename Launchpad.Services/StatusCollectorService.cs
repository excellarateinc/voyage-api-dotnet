using Launchpad.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Launchpad.Models;
using Launchpad.Core;
using Launchpad.Models.Enum;

namespace Launchpad.Services
{
    public class StatusCollectorService : IStatusCollector
    {
        private readonly IEnumerable<IStatusMonitor> _monitors;

        public StatusCollectorService(IEnumerable<IStatusMonitor> monitors)
        {
            _monitors = monitors.ThrowIfNull(nameof(monitors));
        }

        public IEnumerable<StatusAggregateModel> Collect()
        {
            return _monitors.Select(_ => new StatusAggregateModel { Name = _.Name, Status = _.GetStatus(), Type = _.Type });
        }

        public IEnumerable<StatusAggregateModel> Collect(MonitorType type)
        {
            return _monitors.Where(_ => _.Type == type).Select(_ => new StatusAggregateModel { Name = _.Name, Status = _.GetStatus(), Type = _.Type });
        }
    }
}
