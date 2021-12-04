using ChatApp.Application.Interface;
using ChatApp.Application.ViewModels;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Security.Authentication;
using System.Text;

namespace ChatApp.Application.Service
{
    public class RabbitMQService : IRabbitMQService
    {
        public IConnection GetConnection()
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.UserName = "admin";
            factory.Password = "TvvejtLqCylgb3fxPklNAkylomc4a4a5";
            factory.Port = 5671;
            factory.HostName = "96xvcy.stackhero-network.com";
            factory.VirtualHost = "/";
            factory.Ssl = new SslOption
            {
                ServerName = "96xvcy.stackhero-network.com",
                Enabled = true,
                Version = SslProtocols.Tls12
            };
           
            return factory.CreateConnection();
        }

        public bool send(IConnection con, MessageDetailViewModel message, string queue)
        {
            try
            {
                IModel channel = con.CreateModel();
                channel.ExchangeDeclare("messageexchange", ExchangeType.Direct);
                channel.QueueDeclare(queue, true, false, false, null);
                channel.QueueBind(queue, "messageexchange", queue, null);
                var msg = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
                channel.BasicPublish("messageexchange", queue, null, msg);
            }
            catch (Exception) 
            {
            }

            return true;
        }

        public string receive(IConnection con, string myqueue)
        {
            try
            {
                string queue = myqueue;
                IModel channel = con.CreateModel();
                channel.QueueDeclare(queue: queue, durable: true, exclusive: false, autoDelete: false, arguments: null);
                var consumer = new EventingBasicConsumer(channel);
                BasicGetResult result = channel.BasicGet(queue: queue, autoAck: true);
                if (result != null)
                    return Encoding.UTF8.GetString(result.Body);
                else
                    return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
