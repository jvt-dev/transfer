using System.Linq;
using transfer.Core.Entities;
using transfer.Infrastructure.Data;
using transfer.Infrastructure.Repository.Interface;

namespace transfer.Infrastructure.Repository
{
    public class TransferStatusRepository : ITransferStatusRepository
    {
        public TransferStatusRepository(TransferContext transferContext)
        {
            _transferContext = transferContext;
        }

        private readonly TransferContext _transferContext;

        public TransferStatusEntity GetTransferStatusByIdTransferStatus(int idTransferStatus)
        {
            var query = from transferStatus in _transferContext.TransferStatus
                        where transferStatus.IdTransferStatus.Equals(idTransferStatus)
                        select new TransferStatusEntity
                        {
                            IdTransferStatus = transferStatus.IdTransferStatus,
                            Status = transferStatus.Status
                        };

            return query.FirstOrDefault();
        }
    }
}
