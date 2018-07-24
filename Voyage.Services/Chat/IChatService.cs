using System.Collections.Generic;
using System.Threading.Tasks;
using Voyage.Models;

namespace Voyage.Services.Chat
{
    public interface IChatService
    {
        Task<IEnumerable<ChatChannelModel>> GetChannels(string userId);

        Task<ChatChannelModel> CreateChannel(string userId);

        Task<IEnumerable<ChatMessageModel>> GetMessagesByChannelAsync(string userId, int channelId);

        Task<ChatMessageModel> CreateMessage(string userId, ChatMessageModel model);
    }
}
