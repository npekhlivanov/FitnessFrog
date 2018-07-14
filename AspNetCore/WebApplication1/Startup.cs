using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.CookiePolicy;
using Newtonsoft.Json.Serialization;
using System.Diagnostics;

namespace AspNetCoreTest
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
            //Action<DeleteCookieContext> onDelete = (deleteContext) => Debug.WriteLine(deleteContext.CookieName + " deleted");
            void onDelete(DeleteCookieContext deleteContext) => Debug.WriteLine(deleteContext.CookieName + " deleted");

            // set up the in-memory session provider with a default in-memory implementation of IDistributedCache
            services.Configure<CookiePolicyOptions>(options => 
            {
                //options.CheckConsentNeeded = context => true; ?2.1
                options.MinimumSameSitePolicy = SameSiteMode.None;
                options.OnDeleteCookie = onDelete;
                //options.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always; // whether cookies must be http only
            });

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromSeconds(30);
                options.Cookie.HttpOnly = true; // not accessible by client-side script
            });

            services.AddMvc()
                //.SetCompatibilityVersion(CompatibilityVersion.Version_2_1) 
                .AddJsonOptions(options =>
                {
                    var resolver = options.SerializerSettings.ContractResolver;
                    if (resolver != null)
                    {
                        //        var res = resolver as DefaultContractResolver;
                        // If we want to preserve the casing of field names in JSON:
                        //        res.NamingStrategy = null;
                        // default = CamelCaseNamingStrategy
                    }
                    else
                    {
                        options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    }
                });
            services.AddKendo();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Put UseDeveloperExceptionPage before any middleware you want to catch exceptions in, such as app.UseMvc
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                //app.UseHsts(); -> use Microsoft.AspNetCore.App metapackage, requires ASP.NET Core 2.1
            }

            //app.UseHttpsRedirection(); ?2.1
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession(); // must be before Mvc
            //app.UseHttpContextItemsMiddleware(); ?2.1
            app.UseMvc();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
#pragma warning disable CS0618 // Type or member is obsolete
            app.UseKendo(env);
#pragma warning restore CS0618 // Type or member is obsolete
        }
    }
}
