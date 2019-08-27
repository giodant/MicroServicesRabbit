using MicroRabbit.Transfer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MicroRabbit.Transfer.Domain.Interfaces
{
    public interface ITransferRepository
    {
        IQueryable<TransferLog> GetTransferLogs();
        void Add(TransferLog transferLog);
    }
}
