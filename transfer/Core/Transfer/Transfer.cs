using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using transfer.Core.Transfer.Interface;
using transfer.Exceptions;
using transfer.Infrastructure.Repository.Interface;

namespace transfer.Core.Transfer
{
    public class Transfer : ITransfer
    {
        public Transfer(ITransferRepository transferRepository, ITransferLogRepository transferLogRepository, ITransferStatusRepository transferStatusRepository, IFactory factory)
        {
            _transferRepository = transferRepository;
            _transferLogRepository = transferLogRepository;
            _transferStatusRepository = transferStatusRepository;
            _factory = factory;
        }

        private readonly ITransferRepository _transferRepository;
        private readonly ITransferLogRepository _transferLogRepository;
        private readonly ITransferStatusRepository _transferStatusRepository;
        private readonly IFactory _factory;

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

            using (var connection = _factory.Connection())
            {
                using (var channel = connection.CreateModel())
                {

                    channel.QueueDeclare(
                        queue: "transference",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    var stringfiedMessage = JsonConvert.SerializeObject(transfer);
                    var bytesMessage = Encoding.UTF8.GetBytes(stringfiedMessage);

                    channel.BasicPublish(
                        exchange: "",
                        routingKey: "transference",
                        basicProperties: null,
                        body: bytesMessage);
                }
            }

            return transfer.IdTransfer;
        }
    }
}
