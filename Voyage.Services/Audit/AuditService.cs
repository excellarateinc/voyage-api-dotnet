using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Voyage.Core;
using Voyage.Data.Repositories.ActivityAudit;
using Voyage.Models;
using Voyage.Models.Entities;

namespace Voyage.Services.Audit
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

        public IList<ActivityAudit> GetAuditActivityWithinTime(string userName, string path, int timeInMinutes)
        {
            var twentyMinutesFromNow = DateTime.UtcNow.AddMinutes(-20);
            var lastAuditRecord = _activityRepository.GetAll().Where(c => c.UserName == userName && c.Path == path).OrderByDescending(c => c.Date).Take(10);

            var activityWithin20Minutes = new List<ActivityAudit>();
            foreach (var activityAudit in lastAuditRecord)
            {
                if (DateTime.Compare(activityAudit.Date, twentyMinutesFromNow) == 1)
                {
                    activityWithin20Minutes.Add(activityAudit);
                }
            }

            return activityWithin20Minutes;
        }
    }
}
