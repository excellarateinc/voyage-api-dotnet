using System.Security.Claims;
using AutoMapper;
using Voyage.Models.Entities;

namespace Voyage.Models.Map.Profiles
{
    public class PermissionProfile : Profile
    {
        public PermissionProfile()
        {
            CreateMap<RolePermission, PermissionModel>();

            CreateMap<Claim, PermissionModel>()
                .ForMember(_ => _.PermissionValue, opt => opt.MapFrom(src => src.Value))
                .ForMember(_ => _.PermissionType, opt => opt.MapFrom(src => src.Type))
                .ForMember(_ => _.Id, opt => opt.Ignore());
        }
    }
}
