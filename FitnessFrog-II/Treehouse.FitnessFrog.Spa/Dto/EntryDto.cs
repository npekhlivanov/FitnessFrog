using System;
using System.ComponentModel.DataAnnotations;
using Treehouse.FitnessFrog.Shared.Models;
using Treehouse.FitnessFrog.Spa.Utilities;

namespace Treehouse.FitnessFrog.Spa.Dto
{
    public class EntryDto
    {
        public int Id { get; set; }

        [Required]
        [Range(typeof(DateTime), "1.1.2017", "31.12.2018")]
        public DateTime? Date { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int? ActivityId { get; set; }

        [Required]
        [CustomValidation(typeof(ValidationMethods), "ValidateGreaterThanZero")]
        //[Min(0.1)]
        public decimal? Duration { get; set; }

        [Required]
        [Range((int)Entry.IntensityLevel.Low, (int)Entry.IntensityLevel.High)]
        public Entry.IntensityLevel? Intensity { get; set; }

        public bool Exclude { get; set; }

        [MaxLength(200, ErrorMessage = "The Notes field cannot be longer than 200 characters.")]
        public string Notes { get; set; }

        public Entry ToModel()
        {
            var entry = AutoMapper.Mapper.Map<EntryDto, Entry>(this);
            //var entry = new Entry
            //{
            //    Id = Id,
            //    Date = Date.Value,
            //    ActivityId = ActivityId.Value,
            //    Duration = Duration.Value,
            //    Intensity = Intensity.Value,
            //    Exclude = Exclude,
            //    Notes = Notes
            //};

            return entry;
        }
    }
}