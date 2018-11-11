﻿using SignalRWithAuthentication.Infrastructure;
using System;
using System.Collections.Concurrent; // ConcurrentDictionary
using System.Collections.Generic; // List
using System.Threading; // CancellationToken
using System.Threading.Tasks; // Task

namespace SignalRWithAuthentication.Services
{
    public class WebSocketConnectionsService : IWebSocketConnectionsService
    {
        private readonly ConcurrentDictionary<Guid, WebSocketConnection> _connections = new ConcurrentDictionary<Guid, WebSocketConnection>();

        public void AddConnection(WebSocketConnection webSocketConnection)
        {
            _connections.TryAdd(webSocketConnection.Id, webSocketConnection);
        }

        public void RemoveConnection(Guid id)
        {
            _connections.TryRemove(id, out var connection);
        }

        public Task SendToAllAsync(string message, CancellationToken cancellationToken)
        {
            List<Task> connectionsTasks = new List<Task>();
            foreach (WebSocketConnection connection in _connections.Values)
            {
                connectionsTasks.Add(connection.SendAsync(message, cancellationToken));
            }

            return Task.WhenAll(connectionsTasks);
        }
    }
}
