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
    }
}
