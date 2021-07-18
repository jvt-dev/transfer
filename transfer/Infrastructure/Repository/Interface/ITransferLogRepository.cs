using transfer.Core.Entities;

namespace transfer.Infrastructure.Repository.Interface
{
    public interface ITransferLogRepository
    {
        public void Create(int idTransfer, string message = "");
        public TransferLogEntity GetTransferLogByIdTransfer(int idTransfer);
        public void UpdateLogMessage(TransferLogEntity transferLogEntity, string logMessage);
    }
}
