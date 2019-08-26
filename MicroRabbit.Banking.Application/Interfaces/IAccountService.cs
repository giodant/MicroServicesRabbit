using MicroRabbit.Banking.Application.Models;
using MicroRabbit.Banking.Domain.Models;
using System.Linq;

namespace MicroRabbit.Banking.Application.Interfaces
{
    public interface IAccountService
    {
        IQueryable<Account> GetAccounts();
        void Transfer(AccountTransfer accountTransfer);
    }
}
