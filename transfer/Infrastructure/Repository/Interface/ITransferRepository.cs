using System.Collections.Generic;
using transfer.Core.Entities;
using transfer.Core.Transfer;

namespace transfer.Infrastructure.Repository.Interface
{
    public interface ITransferRepository
    {
        public TransferEntity Create(TransferRequest transferRequest);
        public TransferEntity GetTransferByIdTransfer(int idTransfer);
        public IEnumerable<TransferEntity> GetTransferByIdTransferStatus(int idTransferStatus = 5);
        public void UpdateIdTransferStatus(TransferEntity transferEntity, int idTransferStatus);
    }
}
