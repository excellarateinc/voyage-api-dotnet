using System;

namespace Voyage.Models
{
    public class ChatMessageModel
    {
        public int MessageId { get; set; }

        public int ChannelId { get; set; }

        public string Message { get; set; }

        public string Username { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
