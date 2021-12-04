using ChatApp.Application.ViewModels;
using RabbitMQ.Client;

namespace ChatApp.Application.Interface
{
    public interface IRabbitMQService
    {
        IConnection GetConnection();
        bool send(IConnection con, MessageDetailViewModel message, string friendqueue);
        string receive(IConnection con, string myqueue);
    }
}
