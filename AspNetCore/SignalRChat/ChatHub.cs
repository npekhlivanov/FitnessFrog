using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace SignalRChat
{
    //[Authorize] add this attribute to require authorization to access the hub, using Identity; then use Context.User.Identity.Name in the methods instead of a "user" parameter
    // Alternatively, use [Authorize(AuthenticationSchemes=JwtBearerDefaults.AuthenticationScheme)] to require a valid JWT when connecting to the hub

    // Tutorial: Get started with ASP.NET Core SignalR
    // https://docs.microsoft.com/en-us/aspnet/core/tutorials/signalr?view=aspnetcore-2.1&tabs=visual-studio

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

        //private readonly PresenceTracker presenceTracker;
    }
}
