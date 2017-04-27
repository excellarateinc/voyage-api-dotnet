using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Voyage.Models.Enum;

namespace Voyage.Models.Entities
{
    [Table("UserPhone")]
    public class UserPhone
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        [MaxLength(15)]
        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public PhoneType PhoneType { get; set; }
    }
}
