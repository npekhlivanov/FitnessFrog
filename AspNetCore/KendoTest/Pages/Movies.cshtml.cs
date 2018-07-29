﻿using AspNetCoreDataSource;
using AspNetCoreDataSource.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace KendoTest.Pages
{
    public class MoviesModel : PageModel
    {
        public IEnumerable<Movie> MovieList => DataContext.Movies;

        public void OnGet()
        {

        }
    }
}