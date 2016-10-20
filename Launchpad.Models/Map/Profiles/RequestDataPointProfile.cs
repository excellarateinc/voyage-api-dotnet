using AutoMapper;

namespace Launchpad.Models.Map.Profiles
{
    public class RequestDataPointProfile : Profile
    {
        public RequestDataPointProfile()
        {
            CreateMap<RequestDataPointModel, StatusModel>()
                .ForMember(_ => _.Code, opt => opt.UseValue(Enum.StatusCode.OK))
                .ForMember(_ => _.Message, opt => opt.MapFrom(src => src.ToString()))
                .ForMember(_ => _.Time, opt => opt.MapFrom(src => src.RequestDateTime));

        }
    }
}
