using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;

namespace Treehouse.FitnessFrog.Shared.Models
{
    public class User: IdentityUser
    {
        [Required]
        [MaxLength(50, ErrorMessage = "The display name cannot be longer than 50 characters")]
        [Display(Name = "Display name")]
        public string DisplayName { get; set; }
    }
}
