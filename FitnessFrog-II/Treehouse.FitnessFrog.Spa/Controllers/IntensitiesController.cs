using System;
using System.Linq;
using System.Web.Http;
using Treehouse.FitnessFrog.Shared.Models;

namespace Treehouse.FitnessFrog.Spa.Controllers
{
    public class IntensitiesController : ApiController
    {
        // GET api/<controller>
        public IHttpActionResult Get()
        {
            var values = Enum.GetValues(typeof(Entry.IntensityLevel));
            var result = values
                .Cast<Entry.IntensityLevel>()
                .Select(v => new { id = (int)v, name = v.ToString() })
                .OrderBy(r => r.id)
                .ToList();
            return Ok(result);
        }
    }
}