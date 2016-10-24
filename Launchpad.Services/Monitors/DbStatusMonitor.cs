using Launchpad.Services.Interfaces;
using System;
using System.Collections.Generic;
using Launchpad.Models;
using Launchpad.Data.Interfaces;
using Launchpad.Core;
using Launchpad.Models.Enum;

namespace Launchpad.Services.Monitors
{
    public class DbStatusMonitor : IStatusMonitor
    {
        private readonly IEnumerable<IDbConnectionStatus> _connectionStatuses;

        public DbStatusMonitor(IEnumerable<IDbConnectionStatus> connectionStatuses)
        {
            _connectionStatuses = connectionStatuses.ThrowIfNull(nameof(connectionStatuses));
        }

        public string Name => "Database Status";
        
        public MonitorType Type => MonitorType.Database;

        public IEnumerable<StatusModel> GetStatus()
        {
            List<StatusModel> statuses = new List<StatusModel>();

            foreach(var connectionStatus in _connectionStatuses)
            {
                var connected = connectionStatus.Test();
                statuses.Add(new StatusModel
                {
                    Time = DateTime.Now,
                    Code = connected ? Models.Enum.StatusCode.OK : Models.Enum.StatusCode.Critical,
                    Message = string.Format("{0} -> {1}", connectionStatus.DisplayName, connected ? "Connected" : "Failed")
                });
            }
            return statuses;          
        }
    }
}
