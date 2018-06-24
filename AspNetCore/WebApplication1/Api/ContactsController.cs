using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreTest.Models;

namespace AspNetCoreTest.Api
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ContactsController : Controller
    {
        private static List<Contact> _contacts = new List<Contact> {
            new Contact { Id = 1, Name = "Иван Петров", Email = "i.perov@abv.bg", Phone = "0888123456"},
            new Contact { Id = 2, Name = "Стефан Петров", Email = "s.perov@abv.bg", Phone = "0888123457"},
            new Contact { Id = 3, Name = "Георги Петров", Email = "g.perov@abv.bg", Phone = "0888123458"},
        };

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_contacts);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var result = _contacts.FirstOrDefault(c => c.Id == id);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}