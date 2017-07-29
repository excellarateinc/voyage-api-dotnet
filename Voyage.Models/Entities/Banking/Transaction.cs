using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voyage.Models.Entities.Banking
{
    [Table("Transaction")]
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionId { get; set; }

        public int AccountId { get; set; }

        public DateTime Date { get; set; }

        public int Type { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }

        public decimal Balance { get; set; }
    }
}
