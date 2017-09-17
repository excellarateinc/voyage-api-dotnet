using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Voyage.Models.Entities
{
    [Table("ChatChannelMember")]
    public class ChatChannelMember
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ChannelMemberId { get; set; }

        [ForeignKey("Channel")]
        public int ChannelId { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public DateTime CreateDate { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ChatChannel Channel { get; set; }
    }
}
