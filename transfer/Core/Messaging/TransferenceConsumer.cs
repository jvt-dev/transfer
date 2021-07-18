using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using transfer.Core.Entities;
using transfer.Core.Transfer.Interface;
using transfer.Infrastructure.Options;

namespace transfer.Core.Messaging
{
    public class TransferenceConsumer : IHostedService, IDisposable
    {
        public TransferenceConsumer(IOptions<RabbitMqConfiguration> option, ITransfer transfer)
        {
            _configuration = option.Value;
            var factory = new ConnectionFactory
            {
                Uri = new Uri(_configuration.Uri)
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(
                queue: "transference",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            _transfer = transfer;
        }

        private readonly RabbitMqConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly ITransfer _transfer;
        private Timer _timer;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(15));
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (sender, eventArgs) =>
            {
                var contentArray = eventArgs.Body.ToArray();
                var contentString = Encoding.UTF8.GetString(contentArray);
                var message = JsonConvert.DeserializeObject<TransferEntity>(contentString);
                _transfer.Update(message);

                _channel.BasicAck(eventArgs.DeliveryTag, false);
            };

            _channel.BasicConsume("transference", false, consumer);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
