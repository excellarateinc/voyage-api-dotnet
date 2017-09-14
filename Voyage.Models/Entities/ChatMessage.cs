using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Voyage.Models.Entities
{
    [Table("ChatMessage")]
    public class ChatMessage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MessageId { get; set; }

        [ForeignKey("Channel")]
        public int ChannelId { get; set; }

        public string Message { get; set; }

        public string Username { get; set; }

        [ForeignKey("User")]
        public string CreatedBy { get; set; }

        public DateTime CreateDate { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ChatChannel Channel { get; set; }
    }
}
