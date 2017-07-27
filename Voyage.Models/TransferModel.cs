namespace Voyage.Models
{
    public class TransferModel
    {
        public int FromAccountId { get; set; }

        public int ToAccountId { get; set; }

        public decimal Amount { get; set; }

        public string Memo { get; set; }
    }
}
