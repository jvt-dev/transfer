using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using transfer.Infrastructure.Options;
using transfer.Infrastructure.Repository.Interface;

namespace transfer.Core.Messaging
{
    public class TransferenceProducer : IHostedService, IDisposable
    {
        public TransferenceProducer(IOptions<RabbitMqConfiguration> option, ITransferRepository transferRepository)
        {
            _configuration = option.Value;
            _factory = new ConnectionFactory
            {
                Uri = new Uri(_configuration.Uri)
            };
            _transferRepository = transferRepository;
        }

        private readonly ConnectionFactory _factory;
        private readonly RabbitMqConfiguration _configuration;
        private readonly ITransferRepository _transferRepository;
        private Timer _timer;

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(10));
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {

            var transfersInQueue = _transferRepository.GetTransferByIdTransferStatus();

            using (var connection = _factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {

                    channel.QueueDeclare(
                        queue: "transference",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    foreach (var transferEntity in transfersInQueue)
                    {
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

        public Task StopAsync(CancellationToken stoppingToken)
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
