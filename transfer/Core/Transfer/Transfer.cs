using transfer.Core.Messaging.Interface;
using transfer.Core.Transfer.Interface;
using transfer.Exceptions;
using transfer.Infrastructure.Repository.Interface;

namespace transfer.Core.Transfer
{
    public class Transfer : ITransfer
    {
        public Transfer(ITransferRepository transferRepository, ITransferLogRepository transferLogRepository, ITransferStatusRepository transferStatusRepository, ITransferenceProducer transferenceProducer)
        {
            _transferRepository = transferRepository;
            _transferLogRepository = transferLogRepository;
            _transferStatusRepository = transferStatusRepository;
            _transferenceProducer = transferenceProducer;
        }

        private readonly ITransferRepository _transferRepository;
        private readonly ITransferLogRepository _transferLogRepository;
        private readonly ITransferStatusRepository _transferStatusRepository;
        private readonly ITransferenceProducer _transferenceProducer;

        public TransferDto Index(int idTransfer)
        {
            var transfer = _transferRepository.GetTransferByIdTransfer(idTransfer);

            if (transfer == null)
                throw new InvalidTransactionId("Invalid transactionId");

            var transferLog = _transferLogRepository.GetTransferLogByIdTransfer(transfer.IdTransfer);
            var transferStatus = _transferStatusRepository.GetTransferStatusByIdTransferStatus(transfer.IdTransferStatus);

            var dto = new TransferDto
            {
                Status = transferStatus.Status,
                Message = transferLog.LogMessage
            };

            return dto;
        }

        public int Create(TransferRequest transferRequest)
        {
            if (transferRequest.Value <= 0)
                throw new InvalidValue("Value must be greater than zero");

            var transfer = _transferRepository.Create(transferRequest);
            _transferLogRepository.Create(transfer.IdTransfer, "");
            _transferenceProducer.Publish(transfer);

            return transfer.IdTransfer;
        }
    }
}
