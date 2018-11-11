using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalRWithAuthentication.Services
{
    public class PresenceTracker
    {
        private static readonly Dictionary<string, int> _onlineUsers = new Dictionary<string, int>();

        public Task<ConnectionOpenedResult> ConnectionOpened(string userId)
        {
            var joined = false;
            lock(_onlineUsers)
            {
                if (_onlineUsers.ContainsKey(userId))
                {
                    _onlineUsers[userId] += 1;
                }
                else
                {
                    _onlineUsers.Add(userId, 1);
                    joined = true;
                }
            }

            return Task.FromResult(new ConnectionOpenedResult { UserJoined = joined });
        }

        public Task<ConnectionClosedResult> ConnectionClosed(string userId)
        {
            var left = false;
            lock (_onlineUsers)
            {
                if (_onlineUsers.ContainsKey(userId))
                {
                    _onlineUsers[userId] -= 1;
                    if (_onlineUsers[userId] <= 0)
                    {
                        _onlineUsers.Remove(userId);
                        left = true;
                    }
                }
            }

            return Task.FromResult(new ConnectionClosedResult { UserLeft = left });
        }

        public Task<string[]> GetOnlineUsers()
        {
            lock (_onlineUsers)
            {
                var result = new List<string>();
                foreach (var item in _onlineUsers)
                {
                    result.Add($"{item.Key} ({item.Value})");
                }
                return Task.FromResult(result.ToArray());//_onlineUsers.Keys.ToArray()
            }
        }
    }

    public class ConnectionOpenedResult
    {
        public bool UserJoined { get; set; }
    }

    public class ConnectionClosedResult
    {
        public bool UserLeft { get; set; }
    }
}
