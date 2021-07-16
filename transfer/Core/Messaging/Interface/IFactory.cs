using RabbitMQ.Client;

namespace transfer.Core.Messaging.Interface
{
    public interface IFactory
    {
        public IConnection Connection();
    }
}
