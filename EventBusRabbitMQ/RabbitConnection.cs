using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EventBusRabbitMQ
{
  public  class RabbitConnection:IRabbitConnection
    {
        private readonly IConnectionFactory connectionFactory;
        private readonly int retryCount;
        private IConnection connection;
        object sync_root = new object();
        bool disposed;

        public RabbitConnection(IConnectionFactory connectionFactory, int retryCount)
        {
            this.connectionFactory = connectionFactory;
            this.retryCount = retryCount;
        }

        public bool IsConnected
        {
            get
            {
                return connection != null && connection.IsOpen && !disposed;
            }
        }

        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("No Rabbit connection opened");
            }
            return connection.CreateModel();
        }

        public bool TryConnect()
        {
            lock (sync_root)
            {
                connection = connectionFactory.CreateConnection();
                if (IsConnected)
                {
                    return true;
                } else
                {
                    return false;
                }
            }
        }

        public void Dispose()
        {
            if (disposed) return;
            disposed = true;
            try
            {
                connection.Dispose();
            }
            catch (IOException ex)
            {//TODO log
                throw;
            }
        }
    }
}
