using EventBus;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;

namespace EventBusRabbitMQ
{
    public class EventBusRabbitMQ : IEventBus
    {
        private readonly IRabbitConnection rabbitConnection;

        public EventBusRabbitMQ(IRabbitConnection rabbitConnection)
        {
            this.rabbitConnection = rabbitConnection;
        }

        public void Publish(IntegrationEvent @event)
        {
            if (!rabbitConnection.IsConnected)
            {
                rabbitConnection.TryConnect();
            }

            using (var channel = rabbitConnection.CreateModel())
            {
                //TODO Custom Exchange name is not working
                //channel.ExchangeDeclare(exchange: BrokerName,
                //    type: "direct");

                var message = JsonConvert.SerializeObject(@event);
                var body = System.Text.Encoding.UTF8.GetBytes(message);

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.BasicPublish(exchange: "",//using default
                                     routingKey: "ProductPriceChangedQueue",// @event.GetType().Name,
                                     mandatory: true,
                                     basicProperties: properties,
                                     body: body);
            }
        }

        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            throw new NotImplementedException();
        }
    }
}
