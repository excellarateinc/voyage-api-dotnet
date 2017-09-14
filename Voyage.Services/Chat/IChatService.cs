using System.Collections.Generic;
using System.Threading.Tasks;
using Voyage.Models;

namespace Voyage.Services.Chat
{
    public interface IChatService
    {
        IEnumerable<ChatChannelModel> GetChannels(string userId);

        Task<ChatChannelModel> CreateChannel(string userId);

        IEnumerable<ChatMessageModel> GetMessagesByChannel(string userId, int channelId);

        Task<ChatMessageModel> CreateMessage(string userId, ChatMessageModel model);
    }
}
