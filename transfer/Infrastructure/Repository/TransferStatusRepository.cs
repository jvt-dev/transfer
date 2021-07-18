using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using transfer.Core.Entities;
using transfer.Infrastructure.Data;
using transfer.Infrastructure.Repository.Interface;

namespace transfer.Infrastructure.Repository
{
    public class TransferStatusRepository : ITransferStatusRepository
    {
        public TransferStatusRepository(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        private readonly IServiceScopeFactory _scopeFactory;

        public TransferStatusEntity GetTransferStatusByIdTransferStatus(int idTransferStatus)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var transferContext = scope.ServiceProvider.GetRequiredService<TransferContext>();

                var query = from transferStatus in transferContext.TransferStatus
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
}
