using System.Security.Claims;
using AutoMapper;
using Voyage.Models.Entities;

namespace Voyage.Models.Map.Profiles
{
    public class ClaimProfile : Profile
    {
        public ClaimProfile()
        {
            CreateMap<RoleClaim, ClaimModel>();

            CreateMap<Claim, ClaimModel>()
                .ForMember(_ => _.ClaimValue, opt => opt.MapFrom(src => src.Value))
                .ForMember(_ => _.ClaimType, opt => opt.MapFrom(src => src.Type))
                .ForMember(_ => _.Id, opt => opt.Ignore());
        }
    }
}
