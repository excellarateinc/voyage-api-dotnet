using AutoMapper;
using Launchpad.Models.EntityFramework;

namespace Launchpad.Models.Map.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, UserModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.UserName));

            CreateMap<ApplicationUser, UserWithRolesModel>()
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.UserName))
               .ForMember(dest => dest.Roles, opt => opt.Ignore());

            CreateMap<UserModel, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Name))
                .ForAllOtherMembers(opt => opt.Ignore());
        }
    }
}
