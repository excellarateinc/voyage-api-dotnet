﻿using AutoMapper;
using Voyage.Models.Entities;
using Voyage.Models.Enum;

namespace Voyage.Models.Map.Profiles
{
    public class UserPhoneProfile : Profile
    {
        public UserPhoneProfile()
        {
            CreateMap<UserPhone, UserPhoneModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.PhoneType, opt => opt.MapFrom(src => src.PhoneType.ToString()))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.VerificationCode, opt => opt.MapFrom(src => src.VerificationCode));

            CreateMap<UserPhoneModel, UserPhone>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.PhoneType, opt => opt.MapFrom(src => (PhoneType)System.Enum.Parse(typeof(PhoneType), src.PhoneType)))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.VerificationCode, opt => opt.MapFrom(src => src.VerificationCode))
                .ForAllOtherMembers(opt => opt.Ignore());
        }
    }
}
