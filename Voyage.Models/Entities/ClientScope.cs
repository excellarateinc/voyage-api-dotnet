using Embarr.WebAPI.AntiXss;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voyage.Models.Entities
{
    public class ClientScope
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
        public string ClientScopeTypeId { get; set; }

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
