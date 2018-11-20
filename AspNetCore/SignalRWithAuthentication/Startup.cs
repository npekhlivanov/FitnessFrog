using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SignalRWithAuthentication.Data;
using SignalRWithAuthentication.Hubs;
using SignalRWithAuthentication.Services;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Net.WebSockets;

namespace SignalRWithAuthentication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<ApplicationUser>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            #region AspNetCoreIdentityOptions
            // Configure ASP.NET Core Identity options
            services.Configure<IdentityOptions>(options =>
            {
                // Default SignIn settings.
                //options.SignIn.RequireConfirmedEmail = true; // default is false
                options.SignIn.RequireConfirmedPhoneNumber = false;
                // Default Lockout settings, default values are 5, 5, true
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
                // Default Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
                // Default User settings.
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });

            //  ConfigureApplicationCookie must be called after calling AddIdentity or AddDefaultIdentity
            //services.ConfigureApplicationCookie(options =>
            //{
            //    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            //    //options.Cookie.Name = "YourAppCookieName";
            //    options.Cookie.HttpOnly = true;
            //    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
            //    options.LoginPath = "/Identity/Account/Login";
            //    // ReturnUrlParameter requires 
            //    //using Microsoft.AspNetCore.Authentication.Cookies;
            //    options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
            //    options.SlidingExpiration = true;
            //});

            #endregion

            // Configure authorization via JWT token in the query string 
            // See https://www.codemag.com/Article/1807061/Build-Real-time-Applications-with-ASP.NET-Core-SignalR
            var key = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(Configuration["JwtKey"]));
            services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        LifetimeValidator = (before, expires, token, parameters) => expires > DateTime.UtcNow,
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateActor = false,
                        ValidateLifetime = true,
                        IssuerSigningKey = key,
                        NameClaimType = ClaimTypes.NameIdentifier
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            if (!string.IsNullOrEmpty(accessToken))
                            {
                                context.Token = accessToken;
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(JwtBearerDefaults.AuthenticationScheme, policy =>
                {
                    policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireClaim(ClaimTypes.NameIdentifier);
                });
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // The reason we have to add support for CORS, is because the SignalR hub call from client uses OPTIONS method
            // see https://dotnetcorecentral.com/blog/real-time-web-application-in-asp-net-core-signalr/
            //services.AddCors(options => {
            //    options.AddPolicy("CorsPolicy", b => { b.AllowAnyMethod(); });
            //});

            // Add SignalR services to the service collection
            services.AddSignalR();
            // To add SignalR client library, right-click the project, and select Add > Client-Side Library, then select unpkg for Provider .
            // For Library, enter @aspnet/signalr@1, and select the latest version that isn't preview
            // Select Choose specific files, and select signalr.js and signalr.min.js, set Target Location to wwwroot/lib/signalr/
            // https://docs.microsoft.com/en-us/aspnet/core/tutorials/signalr?view=aspnetcore-2.1&tabs=visual-studio
            // To enable Azure SignalR Service in your application, add a call to .AddAzureSignalR()
            // Beforehand, create an Azure SignalR Service instance in the Azure portal, add Microsoft.Azure.SignalR NuGet package and
            // specify the connection string in an application setting named Azure:SignalR:ConnectionString
            // The ASP.NET Core JWT authorization added earlier must be disabled in order for SignalR Service to integrate with this application (cookie authorization is fine)

            // Add WebSocket ConnectionManager service
            services.AddWebSocketConnections();

            services.AddSingleton<PresenceTracker>();

            services.AddSingleton<MessageRelay>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            // Enable WebSockets and add WebSocketsMiddleware
            app.UseWebSockets();
            app.MapWebSocketConnections("/socket"); // maybe pass host to verify origin: IWebHostBuilder.GetSetting(WebHostDefaults.ServerUrlsKey)

            // Use the CORS policy defined previusly
            //app.UseCors("CorsPolicy");

            // Add SignalR to the request execution pipeline passing a callback to configure the hub routes
            app.UseSignalR(builder =>
            {
                builder.MapHub<ChatHub>("/chat");
            });
            // If using Azure SignalR, use the code below instead of the one above
            //app.UseAzureSignalR(builder =>
            //{
            //    builder.MapHub<Chat>("/chat");
            //});

            // The code below activates the relay service
            //app.ApplicationServices.GetService<MessageRelay>();
        }
    }
}
