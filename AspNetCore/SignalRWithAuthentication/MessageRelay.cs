using Microsoft.AspNetCore.SignalR;
using SignalRWithAuthentication.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SignalRWithAuthentication
{
    public class MessageRelay
    {
        public MessageRelay(IHubContext<ChatHub> hubContext)
        {
            Task.Factory.StartNew(() => {
                while (true)
                {
                    hubContext.Clients.All.SendAsync("sync", DateTime.Now.Ticks);
                    Thread.Sleep(1000);
                }
            });
        }
    }
}
