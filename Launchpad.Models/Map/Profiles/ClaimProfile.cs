using AutoMapper;
using Launchpad.Models.EntityFramework;
using System.Security.Claims;

namespace Launchpad.Models.Map.Profiles
{
  
    public class ClaimProfile : Profile
    {
        public ClaimProfile()
        {
            CreateMap<RoleClaim, ClaimModel>();

            CreateMap<Claim, ClaimModel>()
                .ForMember(_ => _.ClaimValue, opt => opt.MapFrom(src => src.Value))
                .ForMember(_ => _.ClaimType, opt => opt.MapFrom(src => src.Type));
        }

        
    }
}
