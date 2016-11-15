using AutoMapper;
using Launchpad.Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launchpad.Models.Map.Profiles
{
    public class PhoneContactProfile : Profile
    {
        public PhoneContactProfile()
        {
            CreateMap<PhoneContact, PhoneContactModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.PhoneType, opt => opt.MapFrom(src => src.PhoneType))
                //.ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId));

            CreateMap<PhoneContactModel, PhoneContact>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.PhoneType, opt => opt.MapFrom(src => src.PhoneType))
                //.ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId));
        }
    }
}
