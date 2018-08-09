using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace SignalRChat
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task GetNotification(string user)
        {
            await Clients.Caller.SendAsync("ReceiveNotification", string.Format("notification for user {0}: {1}", user, DateTime.Now));
        }

        public async Task ProgressNotification(string user, int percent)
        {
            await Clients.User(user).SendCoreAsync("ProgressNotification", new object[] { percent + 10 });
        }
    }
}
