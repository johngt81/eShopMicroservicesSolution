using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Threading.Tasks;

namespace RabbitConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var queueName = "ProductPriceChangedQueue";
            var factory = new ConnectionFactory() { DispatchConsumersAsync = true };
            factory.Uri = new Uri(@"amqp://kowlozas:NdwUKYQe0HIkDvjgDRYWCVOr_RU4bVa1@hornet.rmq.cloudamqp.com/kowlozas");

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                int messageCount = Convert.ToInt16(channel.MessageCount(queueName));
                Console.WriteLine(" Listening to the queue. This channels has {0} messages on the queue", messageCount);

                var consumer = new AsyncEventingBasicConsumer(channel);

                consumer.Received += Consumer_Received;
                channel.BasicConsume(
                    queue: queueName,
                    autoAck: false,
                    consumer: consumer
                    );
                Console.ReadLine();
            }

            static async Task Consumer_Received(object sender, BasicDeliverEventArgs @event)
            {
                var eventName = @event.RoutingKey;
                var message = System.Text.Encoding.UTF8.GetString(@event.Body.ToArray());

                var integrationEvent = JsonConvert.DeserializeObject<ProductPriceChangedIntegrationEvent>(message);
                var handler = new ProductPriceChangedIntegrationEventHandler();
                await handler.Handle(integrationEvent);
            }
        }

        public class ProductPriceChangedIntegrationEvent
        {
            private readonly int productId;
            private readonly decimal oldPrice;
            private readonly decimal newPrice;

            public ProductPriceChangedIntegrationEvent(int productId, decimal oldPrice, decimal newPrice)
            {
                this.productId = productId;
                this.oldPrice = oldPrice;
                this.newPrice = newPrice;
            }
        }

        public class ProductPriceChangedIntegrationEventHandler
        {
            public Task Handle(ProductPriceChangedIntegrationEvent @event)
            {
                throw new NotImplementedException();
            }
        }
    }
}
