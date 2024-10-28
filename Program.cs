using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppRegistrationByEmail
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "guest", 
                Password = "guest"  
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "email_queue", durable: false, exclusive: false, autoDelete: false);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"{DateTime.Now}: Получено сообщение - {message}");
            };

            channel.BasicConsume(queue: "email_queue", autoAck: true, consumer: consumer);

            Console.WriteLine(" Ожидание сообщений...");
            Console.WriteLine(" Нажмите [Enter] для выхода.");
            Console.ReadLine();
        }
    }
}
