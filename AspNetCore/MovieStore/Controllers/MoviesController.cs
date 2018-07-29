using AspNetCoreDataSource;
using AspNetCoreDataSource.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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

        [HttpPut, Route("update")]
        public ActionResult UpdateAlbum([DataSourceRequest] DataSourceRequest request, Movie movie)
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

        [HttpDelete, Route("delete")]
        public ActionResult DeleteAlbum([DataSourceRequest] DataSourceRequest request, Movie movie)
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