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
    [Table("ClientRole")]
    public class ClientRole
    {
        [AntiXss]
        [Required]
        public string ClientId { get; set; }

        [AntiXss]
        [Required]
        public string RoleId { get; set; }
    }
}
