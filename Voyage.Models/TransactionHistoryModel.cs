using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voyage.Models
{
    public class TransactionHistoryModel
    {
        public int AccountId { get; set; }

        public string AccountName { get; set; }

        public IEnumerable<TransactionModel> Transactions { get; set; }
    }
}
