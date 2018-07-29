using AspNetCoreDataSource;
using AspNetCoreDataSource.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace MovieStore.Pages
{
    public class AddMovieModel : PageModel
    {
        [BindProperty]
        public Movie Movie { get; set; }

        public IEnumerable<string> Genres { get; set; } = new string[] { "Drama", "Action", "Sci-Fy", "Comedy" };

        public IActionResult OnPost(Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            DataContext.Movies.Add(movie);
            return RedirectToPage("Movies");
        }
    }
}