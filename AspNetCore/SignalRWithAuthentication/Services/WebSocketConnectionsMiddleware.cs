using Microsoft.AspNetCore.Http; // for HttpContext, StatusCodes
using SignalRWithAuthentication.Infrastructure;
using System;
using System.IO; // for MemoryStream, StreamReader
using System.Net.WebSockets;
using System.Text; // for Encoding
using System.Threading; // for CancellationToken
using System.Threading.Tasks; // for RequestDelegate

namespace SignalRWithAuthentication.Services
{
    public class WebSocketConnectionsMiddleware
    {
        private IWebSocketConnectionsService _connectionsService;
        private readonly RequestDelegate _next;

        public WebSocketConnectionsMiddleware(RequestDelegate next, IWebSocketConnectionsService connectionsService)
        {
            _next = next;
            _connectionsService = connectionsService ?? throw new ArgumentNullException(nameof(connectionsService));
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                await _next.Invoke(context); // can we have a request to the same path and not for a WS connection? Consider using the code on the next line
                //context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return;
            }

            var webSocket = await context.WebSockets.AcceptWebSocketAsync().ConfigureAwait(false);
            var webSocketConnection = new WebSocketConnection(webSocket);
            _connectionsService.AddConnection(webSocketConnection);

            await webSocketConnection.ReceiveMessagesUntilCloseAsync();
            //await Receive(webSocket, /*async*/ (result, serializedMessage) =>
            //{
            //    if (result.MessageType == WebSocketMessageType.Text)
            //    {
            //        //Message message = JsonConvert.DeserializeObject<Message>(serializedMessage, _jsonSerializerSettings);
            //        //await _webSocketHandler.ReceiveAsync(socket, result, message).ConfigureAwait(false);
            //        return;
            //    }
            //});

            if (webSocketConnection.CloseStatus.HasValue)
            {
                // the close handshake shouldn't be completed on prematurely closed connections 
                await webSocket.CloseAsync(webSocketConnection.CloseStatus.Value, webSocketConnection.CloseStatusDescription, CancellationToken.None);
            }

            _connectionsService.RemoveConnection(webSocketConnection.Id);
        }

        private async Task Receive(WebSocket webSocket, Action<WebSocketReceiveResult, string> handleMessage)
        {
            while (webSocket.State == WebSocketState.Open)
            {
                try
                {
                    string message = null;
                    WebSocketReceiveResult result = null;
                    ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024 * 4]);

                    using (var ms = new MemoryStream())
                    {
                        do
                        {
                            result = await webSocket.ReceiveAsync(buffer, CancellationToken.None).ConfigureAwait(false);
                            ms.Write(buffer.Array, buffer.Offset, result.Count);
                        }
                        while (!result.EndOfMessage);

                        ms.Seek(0, SeekOrigin.Begin);

                        using (var reader = new StreamReader(ms, Encoding.UTF8))
                        {
                            message = await reader.ReadToEndAsync().ConfigureAwait(false);
                        }
                    }

                    if (result.MessageType != WebSocketMessageType.Close)
                    {
                        handleMessage(result, message);
                    }
                }
                catch (WebSocketException e)
                {
                    if (e.WebSocketErrorCode == WebSocketError.ConnectionClosedPrematurely)
                    {
                        webSocket.Abort();
                    }
                }
            }

            //await _webSocketHandler.OnDisconnected(socket);
        }

    }
}
