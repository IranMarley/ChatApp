using ChatApp.Application.Interface;
using ChatApp.Application.ViewModels;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;

namespace ChatApp.MVC
{
    public class ChatHub : Hub
    {
        #region Data Members

        private readonly IStockService _stockService;

        static List<UserDetailViewModel> ConnectedUsers = new List<UserDetailViewModel>();
        static List<MessageDetailViewModel> CurrentMessage = new List<MessageDetailViewModel>();

        #endregion

        #region Constructors

        public ChatHub()
        {
            _stockService = DependencyResolver.Current.GetService<IStockService>();
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

        public void SendMessageToAll(string userName, string message)
        {
            message = message.Trim();
            var date = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            // store last 50 messages in cache
            AddMessageinCache(userName, message, date);

            // Broad cast message
            Clients.All.messageReceived(userName, message, date);

            if (message.StartsWith("/"))
            {
                var result = _stockService.GetQuote(message);
                Clients.All.messageReceived(result.UserName, result.Message, date);
            }
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