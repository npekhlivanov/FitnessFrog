using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;

namespace SignalRWithAuthentication.Services
{
    public static class WebSocketMiddlewareExtensions
    {
        public static IApplicationBuilder MapWebSocketConnections(this IApplicationBuilder app, PathString path)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.Map(path, builder => builder.UseMiddleware<WebSocketConnectionsMiddleware>());
        }
    }
}
