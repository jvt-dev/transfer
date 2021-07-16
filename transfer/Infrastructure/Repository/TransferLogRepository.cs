using transfer.Core.Entities;
using transfer.Infrastructure.Data;
using transfer.Infrastructure.Repository.Interface;
using System.Linq;

namespace transfer.Infrastructure.Repository
{
    public class TransferLogRepository : ITransferLogRepository
    {
        public TransferLogRepository(TransferContext transferContext)
        {
            _transferContext = transferContext;
        }

        private readonly TransferContext _transferContext;

        public void Create(int idTransfer, string message)
        {
            var transferLog = new TransferLogEntity
            {
                IdTransfer = idTransfer,
                LogMessage = message
            };
            _transferContext.TransferLog.Add(transferLog);
            _transferContext.SaveChanges();
        }

        public TransferLogEntity GetTransferLogByIdTransfer(int idTransfer)
        {
            var query = from transferLog in _transferContext.TransferLog
                        where transferLog.IdTransfer.Equals(idTransfer)
                        select new TransferLogEntity 
                        { 
                            IdTransferLog = transferLog.IdTransferLog,
                            IdTransfer = transferLog.IdTransfer,
                            LogMessage = transferLog.LogMessage 
                        };

            return query.FirstOrDefault();
        }
    }
}
