using transfer.Core.Entities;

namespace transfer.Core.Transfer.Interface
{
    public interface ITransfer
    {
        public TransferDto Index(int idTransfer);
        public int Create(TransferRequest transferRequest);
        public void Update(TransferEntity transferEntity);
    }
}
