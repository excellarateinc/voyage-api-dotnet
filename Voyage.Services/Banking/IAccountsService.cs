using System.Collections.Generic;

namespace Voyage.Services.Banking
{
    public interface IAccountsService
    {
        IEnumerable<Models.AccountModel> GetAccounts(string userId);

        IEnumerable<Models.TransactionHistoryModel> GetTransactionHistory(string userId);

        void Transfer(Models.TransferModel transfer);
    }
}
