using transfer.Core.Transfer;

namespace transfer.Core.Account.Interface
{
    public interface IAccount
    {
        public AccountDto TransferToDestination(TransferRequest transferRequest);
    }
}
