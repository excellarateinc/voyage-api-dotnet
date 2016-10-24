using Launchpad.Models.Enum;
using System.Collections.Generic;

namespace Launchpad.Models
{
    public class StatusAggregateModel
    {
        public string Name { get; set; }

        public MonitorType Type {get;set;}

        public IEnumerable<StatusModel> Status { get; set; } 
    }
}
