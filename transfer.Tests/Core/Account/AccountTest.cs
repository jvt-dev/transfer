using Moq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using transfer.Core.Account;
using transfer.Core.Services.Interface;
using transfer.Core.Transfer;
using Xunit;

namespace transfer.Tests.Core.AccountTest
{
    public class AccountTest
    {
        [Fact]
        public void TransferToDestinationReturnsInvalidAccountNumber()
        {
            var transferRequest = new TransferRequest
            {
                AccountOrigin = "123",
                AccountDestination = "123",
                Value = 10
            };
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound
            };

            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock.Setup(a => a.AccountQuery("123")).Returns(Task.FromResult(response));

            var account = new Account(accountServiceMock.Object);
            var result = account.TransferToDestination(transferRequest);

            Assert.Equal(8, result.IdTransferStatus);
            Assert.Equal("Invalid account number", result.Message);
        }

        [Fact]
        public void TransferToDestinationReturnsUnavailableService()
        {
            var transferRequest = new TransferRequest
            {
                AccountOrigin = "123",
                AccountDestination = "123",
                Value = 10
            };
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError
            };

            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock.Setup(a => a.AccountQuery("123")).Returns(Task.FromResult(response));

            var account = new Account(accountServiceMock.Object);
            var result = account.TransferToDestination(transferRequest);

            Assert.Equal(5, result.IdTransferStatus);
            Assert.Equal("Unavailable service", result.Message);
        }

        [Fact]
        public void TransferToDestinationReturnsInsufficientFunds()
        {
            var transferRequest = new TransferRequest
            {
                AccountOrigin = "123",
                AccountDestination = "123",
                Value = 10
            };
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.UnprocessableEntity,
                Content = new StringContent("{\"id\":2,\"accountNumber\":\"123\",\"balance\":5.32926}")
            };

            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock.Setup(a => a.AccountQuery("123")).Returns(Task.FromResult(response));

            var account = new Account(accountServiceMock.Object);
            var result = account.TransferToDestination(transferRequest);

            Assert.Equal(8, result.IdTransferStatus);
            Assert.Equal("Insufficient funds", result.Message);
        }

        [Fact]
        public void TransferToDestinationReturnsSucess()
        {
            var transferRequest = new TransferRequest
            {
                AccountOrigin = "123",
                AccountDestination = "123",
                Value = 10
            };
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"id\":2,\"accountNumber\":\"123\",\"balance\":10.32926}")
            };

            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock.Setup(a => a.AccountQuery("123")).Returns(Task.FromResult(response));

            var account = new Account(accountServiceMock.Object);
            var result = account.TransferToDestination(transferRequest);

            Assert.Equal(7, result.IdTransferStatus);
            Assert.Equal("", result.Message);
        }
    }
}
