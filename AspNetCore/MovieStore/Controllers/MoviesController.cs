using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MovieStore.Controllers
{
    [Produces("application/json")]
    [Route("api/Movies")]
    public class MoviesController : Controller
    {
        [HttpGet]
        public IActionResult Index([DataSourceRequest] DataSourceRequest request)
        {
            var result = DataContext.Movies.ToDataSourceResult(request);
            return new JsonResult(result);
        }
    }
}