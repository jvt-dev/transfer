using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;
using transfer.Core.Messaging.Interface;

namespace transfer.Core.Messaging
{
    public class Factory : IFactory
    {
        public Factory(IConfiguration configuration)
        {
            _configuration = configuration;
            _factory = new ConnectionFactory
            {
                Uri = new Uri(_configuration.GetConnectionString("RabbitMQ"))
            };
        }

        private readonly IConfiguration _configuration;
        private readonly ConnectionFactory _factory;

        public IConnection Connection()
        {
            return _factory.CreateConnection();
        }
    }
}
