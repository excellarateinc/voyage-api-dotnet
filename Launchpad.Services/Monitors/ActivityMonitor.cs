using Launchpad.Services.Interfaces;
using System.Collections.Generic;
using Launchpad.Models;
using Launchpad.Models.Enum;
using Launchpad.Core;
using AutoMapper;

namespace Launchpad.Services.Monitors
{
    public class ActivityMonitor : IStatusMonitor
    {
        private readonly IRequestMetricsService _metricsService;
        private readonly IMapper _mapper;

        public string Name => "Activity";

        public MonitorType Type => MonitorType.Activity;

        public ActivityMonitor(IRequestMetricsService metricsService, IMapper mapper)
        {
            _metricsService = metricsService.ThrowIfNull(nameof(metricsService));
            _mapper = mapper.ThrowIfNull(nameof(mapper));
        }
       

        public IEnumerable<StatusModel> GetStatus()
        {
            return _mapper.Map<IEnumerable<StatusModel>>(_metricsService.GetActivity());
        }
    }
}
