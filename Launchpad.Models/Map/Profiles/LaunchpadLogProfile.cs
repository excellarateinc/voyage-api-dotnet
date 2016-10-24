using AutoMapper;
using Launchpad.Models.EntityFramework;
using Launchpad.Models.Map.Resolvers;

namespace Launchpad.Models.Map.Profiles
{
    public class LaunchpadLogProfile : Profile
    {
        public LaunchpadLogProfile()
        {         
            CreateMap<LaunchpadLog, StatusModel>()
                .ForMember(_ => _.Code, opt => opt.ResolveUsing<LogLevelResolver, string>(_ => _.Level)) //This can be moved into a value resolver
                .ForMember(_ => _.Message, opt => opt.MapFrom(src => src.Message))
                .ForMember(_ => _.Time, opt => opt.MapFrom(src => src.TimeStamp));

        }
    }
}
