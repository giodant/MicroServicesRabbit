using MicroRabbit.Transfer.Data.Context;
using MicroRabbit.Transfer.Domain.Interfaces;
using MicroRabbit.Transfer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MicroRabbit.Transfer.Data.Repository
{
    public class TransferRepository : ITransferRepository
    {
        private TransferDbContext _dbContext;

        public TransferRepository(TransferDbContext context)
        {
            _dbContext = context;
        }

        public void Add(TransferLog transferLog)
        {
            try
            {
                _dbContext.TransferLogs.Add(transferLog);
                _dbContext.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IQueryable<TransferLog> GetTransferLogs()
        {
            return _dbContext.TransferLogs.AsQueryable();
        }
    }
}
