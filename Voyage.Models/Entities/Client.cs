using Embarr.WebAPI.AntiXss;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public int AccessTokenValiditySeconds { get; set; }

        [Required]
        public int RefreshTokenValiditySeconds { get; set; }

        public int? FailedLoginAttempts { get; set; }

        public DateTime? ForceTokenExpireDate { get; set; }

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
        public DateTime CreatedDate { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        public virtual ICollection<ClientRole> ClientRoles { get; set; }

        public virtual ICollection<ClientScope> ClientScopes { get; set; }
    }
}
