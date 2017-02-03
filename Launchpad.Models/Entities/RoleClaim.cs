using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Voyage.Models.Entities
{
    [Table("RoleClaim")]
    public class RoleClaim
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Role")]
        public string RoleId { get; set; }

        public virtual ApplicationRole Role { get; set; }

        public string ClaimType { get; set; }

        public string ClaimValue { get; set; }
    }
}