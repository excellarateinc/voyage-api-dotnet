using AutoMapper;
using Voyage.Models.Entities;

namespace Voyage.Models.Map.Profiles
{
    public class ChatProfile : Profile
    {
        public ChatProfile()
        {
            CreateMap<ChatChannel, ChatChannelModel>();

            CreateMap<ChatMessage, ChatMessageModel>()
                .ReverseMap()
                .ForMember(_ => _.User, opt => opt.Ignore())
                .ForMember(_ => _.Channel, opt => opt.Ignore());

            CreateMap<ChatChannelMember, ChatChannelMemberModel>();
        }
    }
}
