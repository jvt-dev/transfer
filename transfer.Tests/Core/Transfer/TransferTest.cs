using Microsoft.Extensions.Logging;
using Moq;
using transfer.Core.Account.Interface;
using transfer.Core.Entities;
using transfer.Core.Transfer;
using transfer.Exceptions;
using transfer.Infrastructure.Repository.Interface;
using Xunit;

namespace transfer.Tests.Core.TransferTest
{
    public class TransferTest
    {
        [Fact]
        public void IndexThrowsInvalidTransactionId()
        {
            var accountMock = new Mock<IAccount>();

            var loggerMock = new Mock<ILogger<Transfer>>();

            var transferRepositoryMock = new Mock<ITransferRepository>();
            transferRepositoryMock.Setup(t => t.GetTransferByIdTransfer(1)).Returns((TransferEntity)null);

            var transferLogRepositoryMock = new Mock<ITransferLogRepository>();

            var transferStatusRepositoryMock = new Mock<ITransferStatusRepository>();

            var transfer = new Transfer(accountMock.Object, loggerMock.Object, transferRepositoryMock.Object, transferLogRepositoryMock.Object, transferStatusRepositoryMock.Object);

            Assert.Throws<InvalidTransactionId>(() => transfer.Index(1));
        }

        [Fact]
        public void IndexReturnsDto()
        {
            var transferEntity = new TransferEntity 
            { 
                IdTransfer = 1, 
                IdTransferStatus = 5 
            };
            var transferLogEntity = new TransferLogEntity
            {
                IdTransferLog = 1,
                IdTransfer = 1,
                LogMessage = "Unavailable service"
            };
            var transferStatusEntity = new TransferStatusEntity
            {
                IdTransferStatus = 5,
                Status = "In Queue"
            };

            var accountMock = new Mock<IAccount>();

            var loggerMock = new Mock<ILogger<Transfer>>();

            var transferRepositoryMock = new Mock<ITransferRepository>();
            transferRepositoryMock.Setup(t => t.GetTransferByIdTransfer(1)).Returns(transferEntity);

            var transferLogRepositoryMock = new Mock<ITransferLogRepository>();
            transferLogRepositoryMock.Setup(t => t.GetTransferLogByIdTransfer(1)).Returns(transferLogEntity);

            var transferStatusRepositoryMock = new Mock<ITransferStatusRepository>();
            transferStatusRepositoryMock.Setup(t => t.GetTransferStatusByIdTransferStatus(5)).Returns(transferStatusEntity);

            var transfer = new Transfer(accountMock.Object, loggerMock.Object, transferRepositoryMock.Object, transferLogRepositoryMock.Object, transferStatusRepositoryMock.Object);
            var result = transfer.Index(1);

            Assert.Equal("In Queue", result.Status);
            Assert.Equal("Unavailable service", result.Message);
        }

        [Fact]
        public void CreateThrowsInvalidValue()
        {
            var transferRequest = new TransferRequest
            {
                AccountOrigin = "455",
                AccountDestination = "486",
                Value = 0
            };

            var accountMock = new Mock<IAccount>();

            var loggerMock = new Mock<ILogger<Transfer>>();

            var transferRepositoryMock = new Mock<ITransferRepository>();

            var transferLogRepositoryMock = new Mock<ITransferLogRepository>();

            var transferStatusRepositoryMock = new Mock<ITransferStatusRepository>();

            var transfer = new Transfer(accountMock.Object, loggerMock.Object, transferRepositoryMock.Object, transferLogRepositoryMock.Object, transferStatusRepositoryMock.Object);

            Assert.Throws<InvalidValue>(() => transfer.Create(transferRequest));
        }

        [Fact]
        public void CreateReturnsIdTransfer()
        {
            var transferRequest = new TransferRequest
            {
                AccountOrigin = "455",
                AccountDestination = "486",
                Value = 10
            };
            var transferEntity = new TransferEntity
            {
                IdTransfer = 1,
            };

            var accountMock = new Mock<IAccount>();

            var loggerMock = new Mock<ILogger<Transfer>>();

            var transferRepositoryMock = new Mock<ITransferRepository>();
            transferRepositoryMock.Setup(t => t.Create(transferRequest)).Returns(transferEntity);

            var transferLogRepositoryMock = new Mock<ITransferLogRepository>();

            var transferStatusRepositoryMock = new Mock<ITransferStatusRepository>();

            var transfer = new Transfer(accountMock.Object, loggerMock.Object, transferRepositoryMock.Object, transferLogRepositoryMock.Object, transferStatusRepositoryMock.Object);
            var result = transfer.Create(transferRequest);

            Assert.Equal(1, result);
        }
    }
}
