using AutoMapper;
using Launchpad.Models.Entities;

namespace Launchpad.Models.Map.Profiles
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
