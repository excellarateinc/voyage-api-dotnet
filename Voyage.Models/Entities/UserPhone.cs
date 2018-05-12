using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public string PhoneType { get; set; }

        public string VerificationCode { get; set; }
    }
}
