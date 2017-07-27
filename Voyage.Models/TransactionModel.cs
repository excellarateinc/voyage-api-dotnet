using System;
using Embarr.WebAPI.AntiXss;

namespace Voyage.Models
{
    public class TransactionModel
    {
        public int TransactionId { get; set; }

        public int AccountId { get; set; }

        public DateTime Date { get; set; }

        public int Type { get; set; }

        [AntiXss]
        public string Description { get; set; }

        public decimal Amount { get; set; }

        public decimal Balance { get; set; }
    }
}
