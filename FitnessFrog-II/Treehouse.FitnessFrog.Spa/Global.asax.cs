using System.Web;
using System.Web.Http;
using Treehouse.FitnessFrog.Shared.Models;
using Treehouse.FitnessFrog.Spa.Dto;

namespace Treehouse.FitnessFrog.Spa
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            // If we are adding WebAPI to a MVC App, this must be called BEFORE the call to the method that configutres MVC Routes 
            GlobalConfiguration.Configure(WebApiConfig.Register);

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<EntryDto, Entry>();
            });
        }
    }
}
