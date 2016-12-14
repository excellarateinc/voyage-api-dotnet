using AutoMapper;
using Launchpad.Core;
using Launchpad.Data.Interfaces;
using Launchpad.Models;
using Launchpad.Models.EntityFramework;
using Launchpad.Services.Interfaces;
// ReSharper disable once StyleCop.SA1208
using System.Threading.Tasks;

namespace Launchpad.Services
{
    public class AuditService : IAuditService
    {
        private readonly IActivityAuditRepository _activityRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public AuditService(IActivityAuditRepository activityRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _activityRepository = activityRepository.ThrowIfNull(nameof(activityRepository));
            _mapper = mapper.ThrowIfNull(nameof(mapper));
            _unitOfWork = unitOfWork.ThrowIfNull(nameof(unitOfWork));
        }

        public void Record(ActivityAuditModel model)
        {
            using (var transaction = _unitOfWork.Begin())
            {
                var auditRecord = _mapper.Map<ActivityAudit>(model);
                _activityRepository.Add(auditRecord);
                _unitOfWork.SaveChanges();
                transaction.Commit();
            }
        }

        public Task RecordAsync(ActivityAuditModel model)
        {
            return Task.Run(() => Record(model));
        }
    }
}
