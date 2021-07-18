using transfer.Core.Entities;
using transfer.Infrastructure.Data;
using transfer.Infrastructure.Repository.Interface;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace transfer.Infrastructure.Repository
{
    public class TransferLogRepository : ITransferLogRepository
    {
        public TransferLogRepository(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        private readonly IServiceScopeFactory _scopeFactory;

        public void Create(int idTransfer, string message = "")
        {
            var transferLog = new TransferLogEntity
            {
                IdTransfer = idTransfer,
                LogMessage = message
            };

            using (var scope = _scopeFactory.CreateScope())
            {
                var transferContext = scope.ServiceProvider.GetRequiredService<TransferContext>();
                transferContext.TransferLog.Add(transferLog);
                transferContext.SaveChanges();
            }
        }

        public TransferLogEntity GetTransferLogByIdTransfer(int idTransfer)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var transferContext = scope.ServiceProvider.GetRequiredService<TransferContext>();

                var query = from transferLog in transferContext.TransferLog
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

        public void UpdateLogMessage(TransferLogEntity transferLogEntity, string logMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var transferContext = scope.ServiceProvider.GetRequiredService<TransferContext>();

                transferContext.Attach(transferLogEntity);
                transferLogEntity.LogMessage = logMessage;
                transferContext.Entry(transferLogEntity).Property(t => t.LogMessage).IsModified = true;
                transferContext.SaveChanges();
            }
        }
    }
}
