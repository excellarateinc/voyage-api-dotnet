using Launchpad.Services.Interfaces;
using System.Collections.Generic;
using Launchpad.Models;
using Launchpad.Models.Enum;
using Launchpad.Data.Interfaces;
using Launchpad.Core;
using AutoMapper;

namespace Launchpad.Services.Monitors
{
    public class LogMonitor : IStatusMonitor
    {
        private ILaunchpadLogRepository _logRepository;
        private IMapper _mapper;
        public LogMonitor(ILaunchpadLogRepository logRepository, IMapper mapper)
        {
            _logRepository = logRepository.ThrowIfNull(nameof(logRepository));
            _mapper = mapper.ThrowIfNull(nameof(mapper));
        }

        public string Name => "Log Monitor";

        public MonitorType Type => MonitorType.Error;

        public IEnumerable<StatusModel> GetStatus()
        {
            return _mapper.Map<IEnumerable<StatusModel>>(_logRepository.GetRecentActivity());
        }
    }
}
