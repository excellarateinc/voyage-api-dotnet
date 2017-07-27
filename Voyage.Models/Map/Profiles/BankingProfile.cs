using AutoMapper;
using Voyage.Models.Entities.Banking;

namespace Voyage.Models.Map.Profiles
{
    public class BankingProfile : Profile
    {
        public BankingProfile()
        {
            CreateMap<Account, AccountModel>();
            CreateMap<Transaction, TransactionModel>();
        }
    }
}
