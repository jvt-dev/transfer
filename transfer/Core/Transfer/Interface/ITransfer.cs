namespace transfer.Core.Transfer.Interface
{
    public interface ITransfer
    {
        public TransferDto Index(int idTransfer);
        public int Create(TransferRequest transferRequest);
    }
}
