using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using transfer.Core.Account.Interface;
using transfer.Core.Services;
using transfer.Core.Services.Interface;
using transfer.Core.Transfer;

namespace transfer.Core.Account
{
    public class Account : IAccount
    {
        public Account(IAccountService accountService)
        {
            _accountService = accountService;
        }

        private readonly IAccountService _accountService;

        public AccountDto TransferToDestination(TransferRequest transferRequest)
        {
            var originAccountResponse = _accountService.AccountQuery(transferRequest.AccountOrigin);
            var destinationAccountResponse = _accountService.AccountQuery(transferRequest.AccountDestination);

            if (originAccountResponse.Result.StatusCode.Equals(HttpStatusCode.NotFound) || destinationAccountResponse.Result.StatusCode.Equals(HttpStatusCode.NotFound))
                return DtoGenerator(HttpStatusCode.NotFound);

            if (originAccountResponse.Result.StatusCode.Equals(HttpStatusCode.InternalServerError) || destinationAccountResponse.Result.StatusCode.Equals(HttpStatusCode.InternalServerError))
                return DtoGenerator(HttpStatusCode.InternalServerError);

            var originAccountData = DataGenerator(originAccountResponse.Result);

            if (originAccountData.Balance < transferRequest.Value)
                return DtoGenerator(HttpStatusCode.UnprocessableEntity);

            var originAccountDebit = new AccountServiceRequest
            {
                AccountNumber = transferRequest.AccountOrigin,
                Value = transferRequest.Value * -1
            };

            var destinationAccountCredit = new AccountServiceRequest
            {
                AccountNumber = transferRequest.AccountDestination,
                Value = transferRequest.Value
            };

            _accountService.AccountCredit(originAccountDebit);
            _accountService.AccountCredit(destinationAccountCredit);

            return DtoGenerator(HttpStatusCode.OK);
        }

        private AccountDto DtoGenerator(HttpStatusCode statusCode)
        {
            if (statusCode == HttpStatusCode.NotFound)
                return new AccountDto { IdTransferStatus = 8, Message = "Invalid account number" };

            if (statusCode == HttpStatusCode.InternalServerError)
                return new AccountDto { IdTransferStatus = 5, Message = "Unavailable service" };

            if (statusCode == HttpStatusCode.UnprocessableEntity)
                return new AccountDto { IdTransferStatus = 8, Message = "Insufficient funds" };

            return new AccountDto { IdTransferStatus = 7, Message = "" };
        }

        private AccountData DataGenerator(HttpResponseMessage responseMessage)
        {
            var result = responseMessage.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<AccountData>(result);
        }
    }
}
