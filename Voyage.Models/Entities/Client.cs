using Embarr.WebAPI.AntiXss;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voyage.Models.Entities
{
    [Table("Client")]
    public class Client
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
        public string ClientIdentifier { get; set; }

        [AntiXss]
        [Required]
        public string ClientSecret { get; set; }

        [AntiXss]
        [Required]
        public string RedirectUri { get; set; }

        [AntiXss]
        [Required]
        public bool IsSecretRequired { get; set; }

        [Required]
        public bool IsScoped { get; set; }

        [Required]
        public bool IsAutoApprove { get; set; }

        [Required]
        public long AccessValidityInSeconds { get; set; }

        [Required]
        public long RefreshTokenValiditySeconds { get; set; }

        public int FailedLoginAttempts { get; set; }

        public DateTime ForceTokenExpireDate { get; set; }

        [Required]
        public bool IsEnabled { get; set; }

        [Required]
        public bool IsAccountLocked { get; set; }

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

        public virtual ICollection<ClientRole> ClientRoles { get; set; }

        public virtual ICollection<ClientScope> ClientScopes { get; set; }
    }
}
