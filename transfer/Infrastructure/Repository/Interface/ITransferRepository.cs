﻿using System.Collections.Generic;
using transfer.Core.Entities;
using transfer.Core.Transfer;

namespace transfer.Infrastructure.Repository.Interface
{
    public interface ITransferRepository
    {
        public TransferEntity Create(TransferRequest transferRequest, int idTransferStatus = 5);
        public TransferEntity GetTransferByIdTransfer(int idTransfer);
        public IEnumerable<TransferEntity> GetTransferByIdTransferStatus(int idTransferStatus = 5);
        public void UpdateIdStatus(TransferEntity transferEntity, int idTransferStatus);
    }
}
