using Embarr.WebAPI.AntiXss;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Voyage.Models.Entities
{
    [Table("SecurityQuestion")]
    public class SecurityQuestion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Required]
        public string Question { get; set; }

        [Required]
        public string CreatedBy { get; set; }

        [Required]
        public string LastModifiedBy { get; set; }

        [Required]
        public DateTime LastModifiedDate { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public bool IsDeleted { get; set; }
    }
}
