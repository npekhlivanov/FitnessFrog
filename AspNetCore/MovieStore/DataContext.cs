using MovieStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieStore
{
    public class DataContext
    {
        public static ICollection<Movie> Movies { get; set; } = new List<Movie>
        {
            new Movie
            {
                Title = "No coutry for old men",
                ReleaseDate = new DateTime(2006, 1, 1),
                Rating = 8,
                Genre = "Thriller"
            },
            new Movie
            {
                Title = "Scent of a woman",
                ReleaseDate = new DateTime(1993, 1, 1),
                Rating = 7,
                Genre = "Drama"
            },
            new Movie
            {
                Title = "2001: A space Odyssey",
                ReleaseDate = new DateTime(1968, 1, 1),
                Rating = 8,
                Genre = "Sci-Fi"
            },
            new Movie
            {
                Title = "Vanilla Sky",
                ReleaseDate = new DateTime(2001, 1, 1),
                Rating = 6,
                Genre = "Drama"
            }
        };
    }
}
