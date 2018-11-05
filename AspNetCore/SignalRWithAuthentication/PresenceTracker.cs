using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWithAuthentication
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
                return Task.FromResult(_onlineUsers.Keys.ToArray());
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
