using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Voyage.Models.Entities
{
    [Table("ProfileImage")]
    public class ProfileImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProfileImageId { get; set; }

        [Required]
        public string UserId { get; set; }

        public string ImageData { get; set; }
    }
}
