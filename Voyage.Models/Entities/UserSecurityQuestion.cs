using Embarr.WebAPI.AntiXss;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Voyage.Models.Entities
{
    [Table("UserSecurityQuestion")]
    public class UserSecurityQuestion
    {
        [AntiXss]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        [ForeignKey("SecurityQuestion")]
        public string QuestionId { get; set; }

        [Required]
        public string Answer { get; set; }

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
