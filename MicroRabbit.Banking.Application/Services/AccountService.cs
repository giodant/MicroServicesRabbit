using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Models;
using MicroRabbit.Banking.Domain.Commands;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Banking.Domain.Models;
using MicroServicesRabbit.Domain.Core.Bus;
using System.Linq;

namespace MicroRabbit.Banking.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IEventBus _eventBus;

        public AccountService(IAccountRepository accountRepository
                ,IEventBus bus)
        {
            _eventBus = bus;
            _accountRepository = accountRepository;
        }

        public IQueryable<Account> GetAccounts()
        {
            return _accountRepository.GetAccounts();
        }

        public void Transfer(AccountTransfer accountTransfer)
        {
            //Create Transfer Command
            var createTransferCommand = new CreateTransferCommand(accountTransfer.FromAccount
                                                                ,accountTransfer.ToAccount
                                                                ,accountTransfer.TransferAmount);

            _eventBus.SendCommand(createTransferCommand);
        }
    }
}
