using System;

namespace Voyage.Models
{
    public class ChatChannelModel
    {
        public int ChannelId { get; set; }

        public string Name { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
