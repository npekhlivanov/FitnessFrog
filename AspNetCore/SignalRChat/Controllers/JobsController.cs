using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace SignalRChat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly IHubContext<ChatHub> _hubContext;

        public JobsController(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public async void DoJob(string user)
        {
            await _hubContext.Clients.All.SendAsync("ProgressNotification", 10);
            System.Threading.Thread.Sleep(2000);
            await _hubContext.Clients.All.SendAsync("ProgressNotification", 20);
            System.Threading.Thread.Sleep(2000);
            await _hubContext.Clients.All.SendAsync("ProgressNotification", 30);
        }
    }
}