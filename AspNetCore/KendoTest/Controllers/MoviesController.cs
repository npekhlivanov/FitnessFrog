using AspNetCoreDataSource;
using AspNetCoreDataSource.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace KendoTest.Controllers
{
    [Produces("application/json")]
    [Route("api/Movies")]
    public class MoviesController : Controller
    {
        // GET: api/Movies
        [HttpGet]
        public IActionResult Get([DataSourceRequest] DataSourceRequest request)
        {
            var result = DataContext.Movies.ToDataSourceResult(request);
            return new JsonResult(result); 
        }

        // GET: api/Movies/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Movies
        [HttpPut, Route("update")]
        public ActionResult UpdateMovie([DataSourceRequest] DataSourceRequest request, Movie movie)
        {
            var dbMovie = DataContext.Movies.FirstOrDefault(m => m.Id == movie.Id);

            if (this.ModelState.IsValid && movie != null)
            {
                dbMovie.Title = movie.Title;
                dbMovie.Genre = movie.Genre;
                dbMovie.ReleaseDate = movie.ReleaseDate;
                dbMovie.Rating = movie.Rating;
            }

            var result = new[] { dbMovie }.ToDataSourceResult(request, this.ModelState);

            return this.Json(result);
        }

        // PUT: api/Movies/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
        [HttpDelete, Route("delete")]
        public ActionResult DeleteMovie([DataSourceRequest] DataSourceRequest request, Movie movie)
        {
            var dbMovie = DataContext.Movies.FirstOrDefault(a => a.Id == movie.Id);

            if (dbMovie != null)
            {
                DataContext.Movies.Remove(dbMovie);
            }

            var result = new[] { dbMovie }.ToDataSourceResult(request, this.ModelState);
            return this.Json(result);
        }
    }
}
