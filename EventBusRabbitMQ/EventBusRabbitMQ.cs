using EventBus;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EventBusRabbitMQ
{
    public class EventBusRabbitMQ : IEventBus
    {
        private readonly IRabbitConnection rabbitConnection;
        private readonly IEventBusSubscriptionManager eventBusSubscriptionManager;
        private readonly string queueName;
        private IModel consumerChannel;
        public EventBusRabbitMQ(IRabbitConnection rabbitConnection, IEventBusSubscriptionManager eventBusSubscriptionManager, string queueName = null)
        {
            consumerChannel = CreateConsumerChannel();
            this.rabbitConnection = rabbitConnection;
            this.eventBusSubscriptionManager = eventBusSubscriptionManager;
            this.queueName = queueName;
        }

        private IModel CreateConsumerChannel()
        {
            var channel = this.rabbitConnection.CreateModel();
            channel.QueueDeclare(queue: "",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            channel.CallbackException += Channel_CallbackException;
            return channel;
        }

        private void Channel_CallbackException(object sender, RabbitMQ.Client.Events.CallbackExceptionEventArgs e)
        {
            throw new NotImplementedException();
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
            if (consumerChannel != null)
            {
                var consumer = new AsyncEventingBasicConsumer(this.consumerChannel);
                consumer.Received += Consumer_Received;
                this.consumerChannel.BasicConsume(
                    queue: queueName,
                    autoAck: false,
                    consumer
                    );
            }
            else
            {
                //TODO log error
            }
        }

        private async System.Threading.Tasks.Task Consumer_Received(object sender, BasicDeliverEventArgs @event)
        {
            var eventName = @event.RoutingKey;
            var message = Encoding.UTF8.GetString(@event.Body.ToArray());

            await ProcessEvent(eventName, message);

            this.consumerChannel.BasicAck(@event.DeliveryTag, multiple: false);
        }

        private async Task ProcessEvent(string eventName, string message)
        {

            /*
            if (!eventBusSubscriptionManager.HasSubscriptionForEvent(eventName))
            {
                //TODO log event
            }

            var eventHandlers = eventBusSubscriptionManager.GetHandlersForEvent(eventName);
            foreach (var eventHandler in eventHandlers)
            {
               // eventHandler.HandlerType
            //Resolve by handler type

            }*/

        }
    }
}
