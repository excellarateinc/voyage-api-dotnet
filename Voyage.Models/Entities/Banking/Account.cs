using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Voyage.Models.Entities.Banking
{
    [Table("Account")]
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountId { get; set; }

        public string UserId { get; set; }

        public string AccountNumber { get; set; }

        public string Name { get; set; }

        public int Type { get; set; }

        public decimal Balance { get; set; }
    }
}
