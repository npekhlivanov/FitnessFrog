using Microsoft.AspNetCore.Http; // for HttpContext, StatusCodes
using SignalRWithAuthentication.Infrastructure;
using System;
using System.Threading; // for CancellationToken
using System.Threading.Tasks; // for RequestDelegate

namespace SignalRWithAuthentication.Services
{
    public class WebSocketConnectionsMiddleware
    {
        private IWebSocketConnectionsService _connectionsService;

        public WebSocketConnectionsMiddleware(RequestDelegate next, IWebSocketConnectionsService connectionsService)
        {
            _connectionsService = connectionsService ?? throw new ArgumentNullException(nameof(connectionsService));
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return;
            }

            var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            var webSocketConnection = new WebSocketConnection(webSocket);
            _connectionsService.AddConnection(webSocketConnection);
            await webSocketConnection.ReceiveMessagesUntilCloseAsync();
            if (webSocketConnection.CloseStatus.HasValue)
            {
                // the close handshake shouldn't be completed on prematurely closed connections 
                await webSocket.CloseAsync(webSocketConnection.CloseStatus.Value, webSocketConnection.CloseStatusDescription, CancellationToken.None);
            }

            _connectionsService.RemoveConnection(webSocketConnection.Id);
        }
    }
}
