using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Voyage.Core;
using Voyage.Data.Repositories.Banking;
using Voyage.Models;

namespace Voyage.Services.Banking
{
    public class AccountsService : IAccountsService
    {
        private readonly IAccountsRepository _accountsRepository;
        private readonly ITransactionsRepository _transactionsRepository;
        private readonly IMapper _mapper;

        public AccountsService(
            IAccountsRepository accountsRepository,
            ITransactionsRepository transactionsRepository,
            IMapper mapper)
        {
            _accountsRepository = accountsRepository.ThrowIfNull(nameof(accountsRepository));
            _transactionsRepository = transactionsRepository.ThrowIfNull(nameof(transactionsRepository));
            _mapper = mapper.ThrowIfNull(nameof(mapper));
        }

        public IEnumerable<AccountModel> GetAccounts(string userId)
        {
            var accounts = _accountsRepository.GetAll()
                .Where(_ => _.UserId == userId).ToList();

            return _mapper.Map<IEnumerable<AccountModel>>(accounts);
        }

        public IEnumerable<TransactionHistoryModel> GetTransactionHistory(string userId)
        {
            throw new System.NotImplementedException();
        }

        public void Transfer(TransferModel transfer)
        {
            throw new System.NotImplementedException();
        }
    }
}