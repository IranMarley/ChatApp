using System;
using System.Collections.Generic;
using System.Linq;
using ChatApp.Application.Service;
using ChatApp.Application.ViewModels;
using Microsoft.AspNet.SignalR;

namespace ChatApp.Infra.WindowsService.Bot
{
    public class BroadcastHub : Hub
    {
        #region Data Members

        private readonly RabbitMQService _rabbitMQService;

        static List<UserDetailViewModel> ConnectedUsers = new List<UserDetailViewModel>();
        static List<MessageDetailViewModel> CurrentMessage = new List<MessageDetailViewModel>();

        #endregion

        #region Constructors

        public BroadcastHub()
        {
            _rabbitMQService = new RabbitMQService();
        }

        #endregion

        #region Methods

        public void Connect(string userName)
        {
            var id = Context.ConnectionId;
            
            if (ConnectedUsers.Count(x => x.ConnectionId == id) == 0)
            {
                ConnectedUsers.Add(new UserDetailViewModel { ConnectionId = id, UserName = userName });

                // send to caller
                Clients.Caller.onConnected(id, userName, ConnectedUsers, CurrentMessage);

                // send to all except caller client
                Clients.AllExcept(id).onNewUserConnected(id, userName);

            }
        }

        public void SendMessageToQueue(string userName, string message)
        {
            var date = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            SendMessageToAll(userName, message, date);

            if (message.StartsWith("/"))
            {
                var con = _rabbitMQService.GetConnection();
                _rabbitMQService.send(con, new MessageDetailViewModel { UserName = userName, Message = message, Date = date }, "all");
            }

        }

        public void SendMessageToAll(string userName, string message, string date)
        {
            // store last 50 messages in cache
            AddMessageinCache(userName, message, date);

            // Broad cast message
            Clients.All.messageReceived(userName, message, date);
        }

        public void SendPrivateMessage(string toUserId, string message)
        {
            message = message.Trim();
            string fromUserId = Context.ConnectionId;

            var toUser = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == toUserId);
            var fromUser = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == fromUserId);

            if (toUser != null && fromUser != null)
            {
                // send to 
                Clients.Client(toUserId).sendPrivateMessage(fromUserId, fromUser.UserName, message, DateTime.Now.ToString("dd/MM/yyyy HH:mm"));

                // send to caller user
                Clients.Caller.sendPrivateMessage(toUserId, fromUser.UserName, message, DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            }

        }

        public override System.Threading.Tasks.Task OnDisconnected(bool stop)
        {
            var item = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (item != null)
            {
                ConnectedUsers.Remove(item);

                var id = Context.ConnectionId;
                Clients.All.onUserDisconnected(id, item.UserName);

            }

            return base.OnDisconnected(stop);
        }
        
        #endregion

        #region private Messages

        private void AddMessageinCache(string userName, string message, string date)
        {
            CurrentMessage.Add(new MessageDetailViewModel { UserName = userName, Message = message, Date = date });

            if (CurrentMessage.Count > 50)
                CurrentMessage.RemoveAt(0);
        }

        #endregion
    }
}