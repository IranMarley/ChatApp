using ChatApp.Application.Interface;
using RabbitMQ.Client;

namespace ChatApp.Application.Service
{
    public class ChatService : IChatService
    {

        #region Fields

        private readonly IRabbitMQService _rabbitMQService;

        #endregion

        #region Constructors

        public ChatService(IRabbitMQService rabbitMQService)
        {
            _rabbitMQService = rabbitMQService;
        }

        #endregion

        #region Methods

        public string ReceiveMsg(string userName)
        {
            IConnection con = _rabbitMQService.GetConnection();
            return _rabbitMQService.receive(con, userName);
        }

        public bool SendMsg(string userName, string message, string friend)
        {
            IConnection con = _rabbitMQService.GetConnection();
                        
            if (message.StartsWith("/"))
            {
                if (message.StartsWith("/stock="))
                {
                    //TODO: Call the stocks api and validate the return

                    return _rabbitMQService.send(con, $"{userName}#APPL.US quote is $93.42 per share", userName);
                }
                else
                {
                    return _rabbitMQService.send(con, $"{userName}#Command is not valid", userName);
                }
            }

            return _rabbitMQService.send(con, $"{userName}&{message}", friend);
        }

        #endregion
    }
}
