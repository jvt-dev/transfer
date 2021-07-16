using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using transfer.Core.Entities;
using transfer.Core.Messaging.Interface;

namespace transfer.Core.Messaging
{
    public class TransferenceProducer : ITransferenceProducer
    {
        public TransferenceProducer(IFactory factory)
        {
            _factory = factory;
        }

        private readonly IFactory _factory;

        public void Publish(TransferEntity transferEntity)
        {
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

                    var stringfiedMessage = JsonConvert.SerializeObject(transferEntity);
                    var bytesMessage = Encoding.UTF8.GetBytes(stringfiedMessage);

                    channel.BasicPublish(
                        exchange: "",
                        routingKey: "transference",
                        basicProperties: null,
                        body: bytesMessage);
                }
            }
        }
    }
}
