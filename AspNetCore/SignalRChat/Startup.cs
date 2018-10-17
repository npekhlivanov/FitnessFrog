using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SignalRChat
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

            // configure authorization via JWT token in the query string 
            //var key = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(Configuration["JwtKey"]));
            //services.AddAuthentication()
            //    .AddJwtBearer(options =>
            //    {
            //        options.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            LifetimeValidator = (before, expires, token, parameters) => 
            //                expires > DateTime.UtcNow,
            //                ValidateAudience = false,
            //                ValidateIssuer = false,
            //                ValidateActor = false,
            //                ValidateLifetime = true,
            //                IssuerSigningKey = key,
            //                NameClaimType = ClaimTypes.NameIdentifier
            //        };

            //        options.Events = new JwtBearerEvents
            //        {
            //            OnMessageReceived = context => 
            //            {
            //                var accessToken = context.Request.Query["access_token"];
            //                if (!string.IsNullOrEmpty(accessToken))
            //                {
            //                    context.Token = accessToken;
            //                }

            //                return Task.CompletedTask;
            //            }
            //        };
            //    });

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy(JwtBearerDefaults.AuthenticationScheme, policy =>
            //    {
            //        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
            //        policy.RequireClaim(ClaimTypes.NameIdentifier);
            //    });
            //});

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // SignalR - related
            services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyMethod().AllowAnyHeader()
                .WithOrigins("http://localhost:55830")
                .AllowCredentials();
            }));
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            // SignalR - related
            app.UseCors("CorsPolicy");
            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHub>("/chathub");
            });

            app.UseMvc();
        }
    }
}
