using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Voyage.Core;
using Voyage.Core.Exceptions;
using Voyage.Data.Repositories.Banking;
using Voyage.Models;
using Voyage.Models.Entities.Banking;
using Voyage.Models.Enum;

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
            var accountsQueryable = _accountsRepository.GetAll()
                .Where(_ => _.UserId == userId);

            var transactionQueryable = _transactionsRepository.GetAll();

            var transactions = (from account in accountsQueryable
                               select new
                               {
                                   account.AccountId,
                                   AccountName = account.Name,
                                   Transactions = transactionQueryable
                                        .Where(_ => _.AccountId == account.AccountId)
                                        .OrderByDescending(_ => _.Date)
                               }).ToList();

            return transactions.Select(_ => new TransactionHistoryModel
            {
                AccountId = _.AccountId,
                AccountName = _.AccountName,
                Transactions = _mapper.Map<IEnumerable<TransactionModel>>(_.Transactions)
            });
        }

        public void Transfer(TransferModel transfer)
        {
            var fromAccount = _accountsRepository.Get(transfer.FromAccountId);
            var toAccount = _accountsRepository.Get(transfer.ToAccountId);

            if (fromAccount == null || toAccount == null)
            {
                throw new BadRequestException("Accounts not valid for transfer");
            }

            if (fromAccount.AccountId == toAccount.AccountId)
            {
                throw new BadRequestException("Can't transfer to the same account");
            }

            if (fromAccount.Balance < transfer.Amount)
            {
                throw new BadRequestException("Funds insufficient for transfer");
            }

            fromAccount.Balance -= transfer.Amount;
            toAccount.Balance += transfer.Amount;

            _accountsRepository.SaveChanges();

            _transactionsRepository.Add(new Transaction
            {
                AccountId = fromAccount.AccountId,
                Amount = transfer.Amount,
                Balance = fromAccount.Balance,
                Type = (int)TransactionType.Withdrawal,
                Date = DateTime.Now,
                Description = $"Transfer to {toAccount.Name}"
            });

            _transactionsRepository.Add(new Transaction
            {
                AccountId = toAccount.AccountId,
                Amount = transfer.Amount,
                Balance = toAccount.Balance,
                Type = (int)TransactionType.Deposit,
                Date = DateTime.Now,
                Description = $"Transfer from {fromAccount.Name}"
            });
        }
    }
}