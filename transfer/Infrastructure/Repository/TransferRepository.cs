using transfer.Core.Entities;
using transfer.Core.Transfer;
using transfer.Infrastructure.Data;
using transfer.Infrastructure.Repository.Interface;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace transfer.Infrastructure.Repository
{
    public class TransferRepository : ITransferRepository
    {
        public TransferRepository(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        private readonly IServiceScopeFactory _scopeFactory;

        public TransferEntity Create(TransferRequest transferRequest)
        {
            var transfer = new TransferEntity
            {
                AccountOrigin = transferRequest.AccountOrigin,
                AccountDestination = transferRequest.AccountDestination,
                IdTransferStatus = 5,
                TransferValue = transferRequest.Value
            };

            using (var scope = _scopeFactory.CreateScope())
            {
                var transferContext = scope.ServiceProvider.GetRequiredService<TransferContext>();
                transferContext.Transfer.Add(transfer);
                transferContext.SaveChanges();

                return transfer;
            }
        }

        public TransferEntity GetTransferByIdTransfer(int idTransfer)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var transferContext = scope.ServiceProvider.GetRequiredService<TransferContext>();

                var query = from transfer in transferContext.Transfer
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

        public IEnumerable<TransferEntity> GetTransferByIdTransferStatus(int idTransferStatus = 5)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var transferContext = scope.ServiceProvider.GetRequiredService<TransferContext>();

                var query = from transfer in transferContext.Transfer
                            where transfer.IdTransferStatus.Equals(idTransferStatus)
                            select new TransferEntity
                            {
                                IdTransfer = transfer.IdTransfer,
                                AccountOrigin = transfer.AccountOrigin,
                                AccountDestination = transfer.AccountDestination,
                                IdTransferStatus = transfer.IdTransferStatus,
                                TransferStatus = transfer.TransferStatus,
                                TransferValue = transfer.TransferValue
                            };

                return query.ToList();
            }
        }

        public void UpdateIdTransferStatus(TransferEntity transferEntity, int idTransferStatus)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var transferContext = scope.ServiceProvider.GetRequiredService<TransferContext>();

                transferContext.Attach(transferEntity);
                transferEntity.IdTransferStatus = idTransferStatus;
                transferContext.Entry(transferEntity).Property(t => t.IdTransferStatus).IsModified = true;
                transferContext.SaveChanges();
            }
        }
    }
}
