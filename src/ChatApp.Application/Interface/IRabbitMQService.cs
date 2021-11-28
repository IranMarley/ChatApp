using RabbitMQ.Client;

namespace ChatApp.Application.Interface
{
    public interface IRabbitMQService
    {
        IConnection GetConnection();
        bool send(IConnection con, string message, string friendqueue);
        string receive(IConnection con, string myqueue);
    }
}
