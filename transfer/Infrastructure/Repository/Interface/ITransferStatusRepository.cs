using transfer.Core.Entities;

namespace transfer.Infrastructure.Repository.Interface
{
    public interface ITransferStatusRepository
    {
        public TransferStatusEntity GetTransferStatusByIdTransferStatus(int idTransferStatus);
    }
}
