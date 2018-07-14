using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreTest.Models
{
    public class Movie
    {
        [Required]
        public string Title { get; set; }

        [Display(Name = "ReleaseDate")]
        public DateTime ReleaseDate { get; set; }

        public string Genre { get; set; }

        [DisplayFormat(DataFormatString = "{0:N1}", ApplyFormatInEditMode = true)]
        [Range(1, 10, ErrorMessage = "The rating should be a number between 1 and 10.")]
        public int Rating { get; set; }

    }
}
