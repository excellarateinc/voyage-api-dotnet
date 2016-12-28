﻿using System.Threading.Tasks;
using Launchpad.Models;
using Launchpad.Core;
using AutoMapper;
using Launchpad.Data.Repositories.ActivityAudit;
using Launchpad.Models.Entities;

namespace Launchpad.Services.Audit
{    
    public class AuditService : IAuditService
    {
        private readonly IActivityAuditRepository _activityRepository;
        private readonly IMapper _mapper;

        public AuditService(IActivityAuditRepository activityRepository, IMapper mapper)
        {            
            _activityRepository = activityRepository.ThrowIfNull(nameof(activityRepository));
            _mapper = mapper.ThrowIfNull(nameof(mapper));
        }

        public void Record(ActivityAuditModel model)
        {
            var auditRecord = _mapper.Map<ActivityAudit>(model);
            _activityRepository.Add(auditRecord);
        }

        public Task RecordAsync(ActivityAuditModel model)
        {
            return Task.Run(() => Record(model));
        }
    }
}
