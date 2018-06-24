using System.Web.Mvc;

namespace Treehouse.FitnessFrog
{
    public static class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new RequireHttpsAttribute()); // configures MVC to redirect any request using HTTP to use HTTPS
            filters.Add(new AuthorizeAttribute()); // this makes all controllers accessible to authorized users only
        }
    }
}