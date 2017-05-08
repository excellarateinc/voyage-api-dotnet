using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Voyage.Models.Entities
{
    [Table("RolePermission")]
    public class RolePermission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Role")]
        public string RoleId { get; set; }

        public virtual ApplicationRole Role { get; set; }

        public string PermissionType { get; set; }

        public string PermissionValue { get; set; }
    }
}