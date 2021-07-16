using transfer.Core.Entities;
using transfer.Core.Transfer;
using transfer.Infrastructure.Data;
using transfer.Infrastructure.Repository.Interface;
using System.Linq;

namespace transfer.Infrastructure.Repository
{
    public class TransferRepository : ITransferRepository
    {
        public TransferRepository(TransferContext transferContext)
        {
            _transferContext = transferContext;
        }

        private readonly TransferContext _transferContext;

        public TransferEntity Create(TransferRequest transferRequest)
        {
            var transfer = new TransferEntity
            {
                AccountOrigin = transferRequest.AccountOrigin,
                AccountDestination = transferRequest.AccountDestination,
                IdTransferStatus = 5,
                TransferValue = transferRequest.Value
            };
            _transferContext.Transfer.Add(transfer);
            _transferContext.SaveChanges();

            return transfer;
        }

        public TransferEntity GetTransferByIdTransfer(int idTransfer)
        {
            var query = from transfer in _transferContext.Transfer
                        where transfer.IdTransfer.Equals(idTransfer)
                        select new TransferEntity
                        {
                            IdTransfer = transfer.IdTransfer,
                            AccountOrigin = transfer.AccountOrigin,
                            AccountDestination = transfer.AccountDestination,
                            IdTransferStatus = transfer.IdTransferStatus,
                            TransferStatus = transfer.TransferStatus,
                            TransferValue = transfer.TransferValue
                        };

            return query.FirstOrDefault();
        }
    }
}
