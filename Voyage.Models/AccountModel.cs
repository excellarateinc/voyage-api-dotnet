using System;
using Embarr.WebAPI.AntiXss;

namespace Voyage.Models
{
    public class AccountModel
    {
        public int AccountId { get; set; }

        [AntiXss]
        public string AccountNumber { get; set; }

        [AntiXss]
        public string Name { get; set; }

        public int Type { get; set; }

        public decimal Balance { get; set; }
    }
}
