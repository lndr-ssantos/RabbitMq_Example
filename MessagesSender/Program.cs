using MessagesSender.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;

namespace MessagesSender
{
    public static class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory 
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "admin",
                Password = "admin"
            };

            var queueName = "OrderQueue";

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                while (true)
                {
                    var order = new Order { Name = $"Order #{new Random().Next(500)}" };

                    var orderJson = JsonConvert.SerializeObject(order);
                    var body = Encoding.UTF8.GetBytes(orderJson);
                    Console.WriteLine(orderJson);

                    channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);

                    Thread.Sleep(20);
                }
            }
        }
    }
}
