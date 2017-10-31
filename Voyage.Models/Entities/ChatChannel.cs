using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Voyage.Models.Entities
{
    [Table("ChatChannel")]
    public class ChatChannel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ChannelId { get; set; }

        public string Name { get; set; }

        [ForeignKey("User")]
        public string CreatedBy { get; set; }

        public DateTime CreateDate { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
