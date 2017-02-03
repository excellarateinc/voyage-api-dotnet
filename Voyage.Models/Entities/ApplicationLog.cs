using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Voyage.Models.Entities
{
    [Table("ApplicationLog")]
    public class ApplicationLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Message { get; set; }

        public string MessageTemplate { get; set; }

        [MaxLength(128)]
        public string Level { get; set; }

        public DateTime TimeStamp { get; set; }

        public string Exception { get; set; }

        public string LogEvent { get; set; }

        [Column(TypeName = "xml")]
        public string Properties { get; set; }
    }
}
