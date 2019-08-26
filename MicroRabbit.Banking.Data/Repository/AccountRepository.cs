using MicroRabbit.Banking.Data.Context;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Banking.Domain.Models;
using System;
using System.Linq;

namespace MicroRabbit.Banking.Data.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private BankingDbContext _dbContext;

        public AccountRepository(BankingDbContext context)
        {
            _dbContext = context;
        }

        public int AddAccount(Account account)
        {
            try
            {
                _dbContext.Accounts.Add(account);
                _dbContext.SaveChanges();
                return account.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IQueryable<Account> GetAccounts()
        {
            return _dbContext.Accounts.AsQueryable();
        }
    }
}
