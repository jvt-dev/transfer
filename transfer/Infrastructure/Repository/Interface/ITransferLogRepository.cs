using transfer.Core.Entities;

namespace transfer.Infrastructure.Repository.Interface
{
    public interface ITransferLogRepository
    {
        public void Create(int idTransfer);
        public TransferLogEntity GetTransferLogByIdTransfer(int idTransfer);
        public void UpdateLogMessage(TransferLogEntity transferLogEntity, string logMessage);
    }
}
