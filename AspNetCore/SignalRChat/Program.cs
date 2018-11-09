using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SignalRChat
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var result = WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

            // Configure the app's web host to use HTTP.sys with Windows authentication
            result = result.UseHttpSys(options =>
            {
                options.Authentication.Schemes = Microsoft.AspNetCore.Server.HttpSys.AuthenticationSchemes.NTLM | Microsoft.AspNetCore.Server.HttpSys.AuthenticationSchemes.Negotiate;
                options.Authentication.AllowAnonymous = false;
            });
            
            return result;
        }
    }
}
