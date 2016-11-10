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

            CreateMap<UserModel, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest=>dest.FirstName, opt=>opt.MapFrom(src=>src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForAllOtherMembers(opt => opt.Ignore());
        }
    }
}
