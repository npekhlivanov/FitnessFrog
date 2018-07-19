using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MovieStore.Pages
{
    public class MoviesModel : PageModel
    {
        public IEnumerable<Movie> MovieList => DataContext.Movies;

        public void OnGet()
        {
        }
    }
}