using SignalRWithAuthentication.Infrastructure;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SignalRWithAuthentication.Services
{
    public interface IWebSocketConnectionsService
    {
        void AddConnection(WebSocketConnection webSocketConnection);
        void RemoveConnection(Guid id);
        Task SendToAllAsync(string message, CancellationToken cancellationToken);
    }
}
