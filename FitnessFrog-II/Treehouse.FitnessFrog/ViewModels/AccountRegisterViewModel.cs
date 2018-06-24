using System.ComponentModel.DataAnnotations;

namespace Treehouse.FitnessFrog.ViewModels
{
    public class AccountRegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "The {0} must be at least {2} characters long.")]
        // The first format item index {0} is the field name, the second index {1} is the maximum length, and the third index {2} is the minimum length
        // Identity provides a much better way to validate user passwords using the PasswordValidator class: 
        // https://msdn.microsoft.com/en-us/library/microsoft.aspnet.identity.passwordvalidator(v=vs.108).aspx
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}