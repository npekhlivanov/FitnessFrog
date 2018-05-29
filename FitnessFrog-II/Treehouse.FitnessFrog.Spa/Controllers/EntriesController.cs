using System.Net;
using System.Net.Http;
using System.Web.Http;
using Treehouse.FitnessFrog.Shared.Data;
using Treehouse.FitnessFrog.Shared.Models;
using Treehouse.FitnessFrog.Spa.Dto;

namespace Treehouse.FitnessFrog.Spa.Controllers
{
    public class EntriesController : ApiController
    {
        private EntriesRepository _entriesRepository = null;

        public EntriesController(EntriesRepository entriesRepository)
        {
            _entriesRepository = entriesRepository;
        }

        [HttpGet]
        public IHttpActionResult GetEntries()
        {
            return Ok(_entriesRepository.GetList());
        }

        // Example for usin HttpResponseMessage
        // For more information on the HttpResponseMessage class, see https://msdn.microsoft.com/en-us/library/system.net.http.httpresponsemessage(v=vs.118).aspx.
        //public HttpResponseMessage Get()
        //{
        //    var entries = _entriesRepository.GetList();
        //    return Request.CreateResponse(HttpStatusCode.OK, entries);
        //}

        // Example for ising DTOs
        //public IHttpActionResult Get()
        //{
        //    var entries = _entriesRepository.GetList();
        //    var entryDtos = entries
        //        .Select(e => new EntryListDto()
        //            {
        //                Id = e.Id,
        //                Date = e.Date,
        //                ActivityName = e.Activity.Name,
        //                Duration = e.Duration,
        //                IntensityName = e.Intensity.ToString()
        //            })
        //        .ToList();

        //    return Ok(entryDtos);
        //}

        [HttpGet]
        public IHttpActionResult GetEntry(int id)
        {
            var entry = _entriesRepository.Get(id);
            if (entry == null)
            {
                return NotFound();
            }

            return Ok(entry);
        }

        [HttpPost]
        public IHttpActionResult Post(EntryDto entry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entryModel = entry.ToModel();
            _entriesRepository.Add(entryModel);
            entry.Id = entryModel.Id;

            return Created(
                Url.Link("DefaultApi", new { controller = "Entries", id = entry.Id }), 
                entry);
        }

        [HttpPut]
        public IHttpActionResult Put(int id, EntryDto entry)
        {
            ValidateEntry(entry);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entryModel = entry.ToModel();
            _entriesRepository.Update(entryModel);

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpDelete]
        //public IHttpActionResult Delete(int id)
        //{
        //    return StatusCode(HttpStatusCode.NotImplemented);
        //}
        public void Delete(int id)
        {
            _entriesRepository.Delete(id);
        }

        private void ValidateEntry(EntryDto entry)
        {
            // If there aren't any "Duration" field validation errors then make sure that the duration is greater than "0".
            if (ModelState.IsValidField("Duration") && entry.Duration <= 0)
            {
                ModelState.AddModelError("entry.Duration", // Web API is prefixing the field names with the parameter name, so we also do it
                    "The Duration field value must be greater than '0'.");
            }
        }
    }
}