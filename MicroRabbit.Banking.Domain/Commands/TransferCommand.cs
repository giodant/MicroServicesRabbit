using MicroServicesRabbit.Domain.Core.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroRabbit.Banking.Domain.Commands
{
    public class TransferCommand : Command
    {
        public TransferCommand()
        {
        }

        public int To { get; protected set; }
        public int From { get; protected set; }
        public decimal Amount { get; protected set; }
    }
}
