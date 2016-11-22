﻿using Launchpad.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Launchpad.Models;
using Launchpad.Data.Interfaces;
using Launchpad.Core;
using AutoMapper;
using Launchpad.Models.EntityFramework;

namespace Launchpad.Services
{
    public class AuditService : IAuditService
    {
        private IActivityAuditRepository _activityRepository;
        private IMapper _mapper;

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