using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Launchpad.Models.EntityFramework
{
    [Table("ActivityAudit")]
    public class ActivityAudit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(64)]
        public string RequestId { get; set; }

        [MaxLength(32)]
        public string Method { get; set; }

        [MaxLength(128)]
        public string Path { get; set; }

        [MaxLength(64)]
        public string IpAddress { get; set; }

        public DateTime Date { get; set; }

        public int StatusCode { get; set; }

        public string Error { get; set; }

        [MaxLength(50)]
        public string UserName { get; set; }
    }
}
