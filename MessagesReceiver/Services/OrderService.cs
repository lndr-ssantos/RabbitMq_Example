using MessegesReceiver.Infra.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace MessegesReceiver.Services
{
    public class OrderService
    {
        private readonly RabbitMqConfiguration _rabbitMqOptions;
        private readonly ILogger<OrderService> _logger;
        private IModel _channel;

        public OrderService(IOptions<RabbitMqConfiguration> rabbitMqOptions, ILogger<OrderService> logger)
        {
            _rabbitMqOptions = rabbitMqOptions.Value;
            _logger = logger;

            InitializeRabbitMqListener();
        }

        public void Execute()
        {
            _logger.LogInformation("Job starting");

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var order = JsonConvert.DeserializeObject(content);

                _logger.LogInformation($"Order received: {order}");
            };

            _channel.BasicConsume(queue: _rabbitMqOptions.QueueName, autoAck: true, consumer: consumer);

            _logger.LogInformation("Processing...");
        }

        private void InitializeRabbitMqListener()
        {
            var factory = new ConnectionFactory
            {
                HostName = _rabbitMqOptions.HostName,
                Port = _rabbitMqOptions.Port,
                UserName = _rabbitMqOptions.UserName,
                Password = _rabbitMqOptions.Password
            };

            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();
            _channel.QueueDeclare(queue: _rabbitMqOptions.QueueName, durable: true, exclusive: false,
                autoDelete: false, arguments: null);
        }
    }
}
