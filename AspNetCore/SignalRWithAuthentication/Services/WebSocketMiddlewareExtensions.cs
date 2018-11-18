using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace SignalRWithAuthentication.Services
{
    public static class WebSocketMiddlewareExtensions
    {
        public static IApplicationBuilder MapWebSocketConnections(this IApplicationBuilder app, PathString path)
        {
            return app.Map(path, builder => builder.UseMiddleware<WebSocketConnectionsMiddleware>());
        }
    }
}
