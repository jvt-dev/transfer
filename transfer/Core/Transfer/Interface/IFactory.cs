using RabbitMQ.Client;

namespace transfer.Core.Transfer.Interface
{
    public interface IFactory
    {
        public IConnection Connection();
    }
}
