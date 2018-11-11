using Microsoft.AspNetCore.SignalR;
using SignalRWithAuthentication.Hubs;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SignalRWithAuthentication.Services
{
    public class MessageRelay
    {
        public MessageRelay(IHubContext<ChatHub> hubContext)
        {
            Task.Factory.StartNew(() => {
                while (true)
                {
                    hubContext.Clients.All.SendAsync("sync", DateTime.Now.ToString("dd.MM.yyyy HH.mm.ss.fff"));
                    Thread.Sleep(1000);
                }
            });
        }
    }
}
