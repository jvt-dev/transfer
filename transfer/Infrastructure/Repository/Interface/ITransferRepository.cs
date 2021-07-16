using transfer.Core.Entities;
using transfer.Core.Transfer;

namespace transfer.Infrastructure.Repository.Interface
{
    public interface ITransferRepository
    {
        public TransferEntity Create(TransferRequest transferRequest);
        public TransferEntity GetTransferByIdTransfer(int idTransfer);
    }
}
