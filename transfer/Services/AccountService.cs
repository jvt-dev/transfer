using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using transfer.Core.Services.Interface;
using transfer.Infrastructure.Options;

namespace transfer.Core.Services
{
    public class AccountService : IAccountService
    {
        public AccountService(IHttpClientFactory clientFactory, IOptions<AccountServiceConfiguration> option)
        {
            _clientFactory = clientFactory;
            _configuration = option.Value;
        }

        private readonly IHttpClientFactory _clientFactory;
        private readonly AccountServiceConfiguration _configuration;

        public Task<HttpResponseMessage> AccountQuery(string accountNumber)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_configuration.Url}/{accountNumber}");
            var client = _clientFactory.CreateClient();
            var response = client.SendAsync(request);

            return response;
        }

        public void AccountCredit(AccountServiceRequest accountServiceRequest)
        {
            var content = new StringContent(JsonSerializer.Serialize(accountServiceRequest), Encoding.UTF8, "application/json");
            var client = _clientFactory.CreateClient();
            client.PostAsync(_configuration.Url, content);
        }
    }
}
