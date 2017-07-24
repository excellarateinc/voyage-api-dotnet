using Embarr.WebAPI.AntiXss;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Voyage.Models.Entities
{
    [Table("ClientRole")]
    public class ClientRole
    {
        [AntiXss]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [AntiXss]
        [Required]
        public string ClientId { get; set; }

        [AntiXss]
        [Required]
        public string RoleId { get; set; }
    }
}
