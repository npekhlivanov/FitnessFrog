using System;
using System.IO; // MemoryStream
using System.Net.WebSockets; // WebSocket, WebSocketCloseStatus, WebSocketReceiveResult
using System.Text; // Encoding
using System.Threading; // CancellationToken
using System.Threading.Tasks; // Task

namespace SignalRWithAuthentication.Infrastructure
{
    /// <summary>
    /// A WebSocket abstraction, allows for sending and receiving messages.
    /// It also provides the receiving loop for the middleware to wait on, together with information required to complete the close handshake.
    /// </summary>
    public class WebSocketConnection
    {
        #region Fields
        private readonly WebSocket _webSocket;

        private readonly int _receivePayloadBufferSize = 4 * 1024;
        #endregion

        #region Properties
        public Guid Id => Guid.NewGuid(); //{ get; internal set; }

        public WebSocketCloseStatus? CloseStatus { get; internal set; } = null;

        public string CloseStatusDescription { get; internal set; } = null;
        #endregion

        #region Events
        public event EventHandler<string> OnReceiveText;

        public event EventHandler<byte[]> OnReceiveBinary; 
        #endregion

        public WebSocketConnection(WebSocket webSocket)
        {
            _webSocket = webSocket ?? throw new ArgumentNullException(nameof(webSocket)); 
        }

        public Task SendAsync(string message, CancellationToken cancellationToken)
        {
            if (_webSocket.State == WebSocketState.Open)
            {
                var messageBytes = Encoding.UTF8.GetBytes(message);
                return _webSocket.SendAsync(new ArraySegment<byte>(messageBytes), WebSocketMessageType.Binary, true, CancellationToken.None);
            }
            else
            {
                return Task.FromResult(false);
            }
        }

        public Task SendAsync(byte[] message, CancellationToken cancellationToken)
        {
            if (_webSocket.State == WebSocketState.Open)
            {
                return _webSocket.SendAsync(new ArraySegment<byte>(message), WebSocketMessageType.Binary, true, CancellationToken.None);
            }
            else
            {
                return Task.FromResult(false);
            }

        }

        public async Task ReceiveMessagesUntilCloseAsync()
        {
            // see https://www.tpeczek.com/2018/02/back-to-websockets-fundamentals-in.html
            // and https://radu-matei.com/blog/aspnet-core-websockets-middleware/
            try
            {
                var receivePayloadBuffer = new byte[_receivePayloadBufferSize];
                var webSocketReceiveResult = await _webSocket.ReceiveAsync(new ArraySegment<byte>(receivePayloadBuffer), CancellationToken.None);

                // receiving loop
                while (webSocketReceiveResult.MessageType != WebSocketMessageType.Close)
                {
                    byte[] webSocketMessage = await ReceiveMessagePayloadAsync(webSocketReceiveResult, receivePayloadBuffer);
                    if (webSocketReceiveResult.MessageType == WebSocketMessageType.Binary)
                    {
                        OnReceiveBinary?.Invoke(this, webSocketMessage);
                    }
                    else
                    {
                        var message = Encoding.UTF8.GetString(webSocketMessage);
                        OnReceiveText?.Invoke(this, message);
                    }

                    webSocketReceiveResult = await _webSocket.ReceiveAsync(new ArraySegment<byte>(receivePayloadBuffer), CancellationToken.None);
                }

                // Close message received, pass CloseStatus and CloseStatusDescription from that message to the waiting middleware to complete the handshake
                CloseStatus = webSocketReceiveResult.CloseStatus.Value;
                CloseStatusDescription = webSocketReceiveResult.CloseStatusDescription;
            }
            catch (WebSocketException ex)
            //when (ex.WebSocketErrorCode == WebSocketError.ConnectionClosedPrematurely)
            {
               CloseStatusDescription = ex.Message;
            }
        }

        private async Task<byte[]> ReceiveMessagePayloadAsync(WebSocketReceiveResult webSocketReceiveResult, byte[] receivePayloadBuffer)
        {
            byte[] messagePayload = null;

            if (webSocketReceiveResult.EndOfMessage)
            {
                messagePayload = new byte[webSocketReceiveResult.Count];
                Array.Copy(receivePayloadBuffer, messagePayload, webSocketReceiveResult.Count);
            }
            else
            {
                using (var messagePayloadStream = new MemoryStream())
                {
                    messagePayloadStream.Write(receivePayloadBuffer, 0, webSocketReceiveResult.Count);
                    while (!webSocketReceiveResult.EndOfMessage)
                    {
                        webSocketReceiveResult = await _webSocket.ReceiveAsync(new ArraySegment<byte>(receivePayloadBuffer), CancellationToken.None);
                        messagePayloadStream.Write(receivePayloadBuffer, 0, webSocketReceiveResult.Count);
                    }

                    messagePayload = messagePayloadStream.ToArray();
                }
            }

            return messagePayload;
        }
    }
}