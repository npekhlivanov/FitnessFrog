using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace SignalRWithAuthentication.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //[Authorize]
    public class ChatHub : Hub
    {
        private readonly PresenceTracker _presenceTracker;

        public ChatHub(PresenceTracker presenceTracker)
        {
            _presenceTracker = presenceTracker;
        }

        public override async Task OnConnectedAsync()
        {
            var result = await _presenceTracker.ConnectionOpened(Context.User.Identity.Name);
            if (result.UserJoined)
            {
                await Clients.Others.SendAsync("newMessage", "system", $"{Context.User.Identity.Name} joined");
            }

            var currentUsers = await _presenceTracker.GetOnlineUsers();
            await Clients.Caller.SendAsync("newMessage", "system", $"Currently online:\n{string.Join("\n", currentUsers)}");

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var result = await _presenceTracker.ConnectionClosed(Context.User.Identity.Name);
            if (result.UserLeft)
            {
                await Clients.All.SendAsync("newMessage", "system", $"{Context.User.Identity.Name} left");
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("newMessage", Context.User.Identity.Name, message);
        }
    }
}
