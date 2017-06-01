using Embarr.WebAPI.AntiXss;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Voyage.Models.Entities
{
    [Table("ClientScopeType")]
    public class ClientScopeType
    {
        [AntiXss]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [AntiXss]
        [Required]
        public string Name { get; set; }

        [AntiXss]
        [Required]
        public string Description { get; set; }

        [AntiXss]
        [Required]
        public string CreatedBy { get; set; }

        [AntiXss]
        [Required]
        public string LastModifiedBy { get; set; }

        [Required]
        public DateTime LastModifiedDate { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        [Required]
        public bool IsDeleted { get; set; }
    }
}
