﻿using System;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreDataSource.Models
{
    public class Movie
    {
        public int Id { get; set; }

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
