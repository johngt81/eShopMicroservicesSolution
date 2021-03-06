using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Basket.API.Infraestructure;
using Basket.API.IntegrationEvents;
using Basket.API.Model;
using EventBus;
using EventBusRabbitMQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Basket.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddAutofac();
            services.AddControllers();
            services.AddDbContext<BasketContext>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            services.AddSingleton<IRabbitConnection>(sp =>
            {
                var factory = new ConnectionFactory();
                factory.Uri = new Uri(@"amqp://kowlozas:NdwUKYQe0HIkDvjgDRYWCVOr_RU4bVa1@hornet.rmq.cloudamqp.com/kowlozas");

                return new RabbitConnection(factory, 3);
            });
            var subscriptionClientName = "client";

            services.AddSingleton<IEventBus, EventBusRabbitMQ.EventBusRabbitMQ>(
                sp =>
                {
                    var rabbitConnection = sp.GetRequiredService<IRabbitConnection>();
                    var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                    var eventBusSubscriptionManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();
                    return new EventBusRabbitMQ.EventBusRabbitMQ(rabbitConnection, iLifetimeScope, eventBusSubscriptionManager, subscriptionClientName, 3);

                });


            var container = new ContainerBuilder();
            container.Populate(services);

            var provider = new AutofacServiceProvider(container.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            ConfigureEventBus(app);
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<ProductPriceChangedIntegrationEvent, ProductPriceChangedIntegrationEventHandler>();

            //var factory = new ConnectionFactory() { DispatchConsumersAsync = true };
            //factory.Uri = new Uri(@"amqp://kowlozas:NdwUKYQe0HIkDvjgDRYWCVOr_RU4bVa1@hornet.rmq.cloudamqp.com/kowlozas");

            //using (var connection = factory.CreateConnection())
            //using (var channel = connection.CreateModel())
            //{
            //    channel.QueueDeclare(queue: "ProductPriceChangedQueue",
            //        durable: true,
            //        exclusive: false,
            //        autoDelete: false,
            //        arguments: null);

            //    var consumer = new AsyncEventingBasicConsumer(channel);

            //    consumer.Received += Consumer_Received;
            //    channel.BasicConsume(
            //        queue: "ProductPriceChangedQueue",
            //        autoAck: false,
            //        consumer: consumer
            //        );
            //}
        }

        //private static async Task Consumer_Received(object sender, BasicDeliverEventArgs @event)
        //{
        //    var eventName = @event.RoutingKey;
        //    var message = System.Text.Encoding.UTF8.GetString(@event.Body.ToArray());

        //    var integrationEvent = JsonConvert.DeserializeObject<ProductPriceChangedIntegrationEvent>(message);
        //    var handler = new ProductPriceChangedIntegrationEventHandler();
        //    await handler.Handle(integrationEvent);
        //}
    }
}
