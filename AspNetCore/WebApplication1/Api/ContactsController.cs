using MovieStore.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

namespace MovieStore.Api
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ContactsController : Controller
    {
        //[HttpGet]
        //public IActionResult Get()
        //{
        //    return Ok(ContactsRepository.GetContacts());
        //}

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var result = ContactsRepository.GetContacts().FirstOrDefault(c => c.Id == id);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet]
        public DataSourceResult Get([DataSourceRequest]DataSourceRequest request)
        {
            var contacts = ContactsRepository.GetContacts();
            var result = contacts.ToDataSourceResult(request);
            return result;
        }
    }
}