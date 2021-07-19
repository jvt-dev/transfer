using Microsoft.Extensions.Logging;
using transfer.Core.Account.Interface;
using transfer.Core.Entities;
using transfer.Core.Transfer.Interface;
using transfer.Exceptions;
using transfer.Infrastructure.Repository.Interface;

namespace transfer.Core.Transfer
{
    public class Transfer : ITransfer
    {
        public Transfer(IAccount account, ILogger<Transfer> logger, ITransferRepository transferRepository, ITransferLogRepository transferLogRepository, ITransferStatusRepository transferStatusRepository)
        {
            _account = account;
            _logger = logger;
            _transferRepository = transferRepository;
            _transferLogRepository = transferLogRepository;
            _transferStatusRepository = transferStatusRepository;
        }

        private readonly IAccount _account;
        private readonly ILogger _logger;
        private readonly ITransferRepository _transferRepository;
        private readonly ITransferLogRepository _transferLogRepository;
        private readonly ITransferStatusRepository _transferStatusRepository;

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

            _logger.LogInformation($"Status: {dto.Status}, Message: {dto.Message}, Created at: {System.DateTime.Now}");

            return dto;
        }

        public int Create(TransferRequest transferRequest)
        {
            if (transferRequest.Value <= 0)
                throw new InvalidValue("Value must be greater than zero");

            var transfer = _transferRepository.Create(transferRequest);
            _transferLogRepository.Create(transfer.IdTransfer);

            return transfer.IdTransfer;
        }

        public void Update(TransferEntity transferEntity)
        {
            var transferRequest = new TransferRequest
            {
                AccountOrigin = transferEntity.AccountOrigin,
                AccountDestination = transferEntity.AccountDestination,
                Value = transferEntity.TransferValue
            };

            var accountDto = _account.TransferToDestination(transferRequest);
            _transferRepository.UpdateIdTransferStatus(transferEntity, accountDto.IdTransferStatus);

            var transferLogEntity = _transferLogRepository.GetTransferLogByIdTransfer(transferEntity.IdTransfer);
            _transferLogRepository.UpdateLogMessage(transferLogEntity, accountDto.Message);
        }
    }
}
