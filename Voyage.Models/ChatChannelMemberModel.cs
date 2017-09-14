using System;

namespace Voyage.Models
{
    public class ChatChannelMemberModel
    {
        public int ChannelMemberId { get; set; }

        public int ChannelId { get; set; }

        public string UserId { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
