using AutoMapper;
using Voyage.Models.Entities;

namespace Voyage.Models.Map.Profiles
{
    public class ActivityAuditProfile : Profile
    {
        public ActivityAuditProfile()
        {
            CreateMap<ActivityAuditModel, ActivityAudit>()
                .ForMember(_ => _.Id, opt => opt.Ignore());
        }
    }
}
