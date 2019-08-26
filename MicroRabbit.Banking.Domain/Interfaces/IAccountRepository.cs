using MicroRabbit.Banking.Domain.Models;
using System.Linq;

namespace MicroRabbit.Banking.Domain.Interfaces
{
    public interface IAccountRepository
    {
        IQueryable<Account> GetAccounts();
        int AddAccount(Account account);
    }
}
