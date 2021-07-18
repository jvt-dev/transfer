using System.Net.Http;
using System.Threading.Tasks;

namespace transfer.Core.Services.Interface
{
    public interface IAccountService
    {
        public Task<HttpResponseMessage> AccountQuery(string accountNumber);
        public void AccountCredit(AccountServiceRequest accountServiceRequest);
    }
}
