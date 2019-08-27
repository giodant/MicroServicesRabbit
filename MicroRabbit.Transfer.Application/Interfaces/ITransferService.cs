using MicroRabbit.Transfer.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace MicroRabbit.Transfer.Application.Interfaces
{
    public interface ITransferService
    {
        IEnumerable<TransferLog> GetTransferLogs();
    }
}
