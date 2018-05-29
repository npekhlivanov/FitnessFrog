using System.Linq;
using System.Web.Http;
using Treehouse.FitnessFrog.Shared.Data;

namespace Treehouse.FitnessFrog.Spa.Controllers
{
    public class ActivitiesController : ApiController
    {
        private ActivitiesRepository _activitiesRepository = null;

        public ActivitiesController(ActivitiesRepository activitiesRepository)
        {
            _activitiesRepository = activitiesRepository;
        }

        public IHttpActionResult Get()
        {
            var result = _activitiesRepository.GetList()
                .OrderBy(a =>a.Id)
                .Select(a => new { id = a.Id, name = a.Name })
                .ToList();
            return Ok(result);
        }
    }
}
