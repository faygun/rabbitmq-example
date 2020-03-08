using Common;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StandartQueue
{
    class Program
    {
        private static IConnectionFactory _factory;
        private static IConnection _connection;
        private static IModel _model;

        private const string QueueName = "StandartQueue_ExampleQueue";
        static void Main(string[] args)
        {
            var payment = new Payment { AmountToPay = 12.0m, CardNumber = "121212121212121", Name = "Mr Faruk" };
            var payment2 = new Payment { AmountToPay = 102.0m, CardNumber = "121212121212121", Name = "Mr Faruk" };
            var payment3 = new Payment { AmountToPay = 300.0m, CardNumber = "121212121212121", Name = "Mr Faruk" };
            var payment4 = new Payment { AmountToPay = 10.30m, CardNumber = "121212121212121", Name = "Mr Faruk" };
            var payment5 = new Payment { AmountToPay = 2.50m, CardNumber = "121212121212121", Name = "Mr Faruk" };
            var payment6 = new Payment { AmountToPay = 1.80m, CardNumber = "121212121212121", Name = "Mr Faruk" };
            var payment7 = new Payment { AmountToPay = 402.0m, CardNumber = "121212121212121", Name = "Mr Faruk" };
            var payment8 = new Payment { AmountToPay = 112.0m, CardNumber = "121212121212121", Name = "Mr Faruk" };
            var payment9 = new Payment { AmountToPay = 12.4m, CardNumber = "121212121212121", Name = "Mr Faruk" };
            var payment10 = new Payment { AmountToPay = 19.99m, CardNumber = "121212121212121", Name = "Mr Faruk" };

            CreateQueue();

            SendMessage(payment);
            SendMessage(payment2);
            SendMessage(payment3);
            SendMessage(payment4);
            SendMessage(payment5);
            SendMessage(payment6);
            SendMessage(payment7);
            SendMessage(payment8);
            SendMessage(payment9);
            SendMessage(payment10);

            Thread.Sleep(1000 * 5);

            Receive();

            Console.ReadLine();
        }

        private static void Receive()
        {
            var consumer = new QueueingBasicConsumer(_model);
            var msgCount = GetMessageCount(_model, QueueName);

            _model.BasicConsume(QueueName, true, consumer);

            var count = 0;

            while (count < msgCount)
            {
                var message = (Payment)consumer.Queue.Dequeue().Body.Deserialize(typeof(Payment));
                Console.WriteLine("-----------Received {0} {1} {2}", message.CardNumber, message.AmountToPay, message.Name);
                count++;
            }
        }

        private static uint GetMessageCount(IModel channel, string queueName)
        {
            var results = channel.QueueDeclare(queueName, true, false, false, null);
            return results.MessageCount;
        }

        private static void SendMessage(Payment message)
        {
            _model.BasicPublish("", QueueName, null, message.Serialize());
            Console.WriteLine("[X]Payment message Sent : {0} : {1} : {2}",  message.CardNumber, message.AmountToPay, message.Name);
        }

        private static void CreateQueue()
        {
            _factory = new ConnectionFactory() { HostName = "localhost", UserName = "guest", Password = "guest" };
            _connection = _factory.CreateConnection();
            _model = _connection.CreateModel();

            _model.QueueDeclare(QueueName, true, false, false, null);
        }
    }
}
