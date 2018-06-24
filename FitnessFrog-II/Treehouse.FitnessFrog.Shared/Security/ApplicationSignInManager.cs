using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Treehouse.FitnessFrog.Shared.Models;

namespace Treehouse.FitnessFrog.Shared.Security
{
    /// <summary>
    /// provide methods to sign-in a user using an instance of the User model class or using a user's key and secret. 
    /// The Identity SignInManager class internally uses an instance of the Identity AuthenticationManager class to sign-in users
    /// </summary>
    public class ApplicationSignInManager : SignInManager<User, string>
    {
        // Instances of the user and authentication managers will be provided by the DI container
        // The IAuthenticationManager concrete implementation provides a property for the current user, a method to sign-in a user (though 
        // remember that we're using the SignInManager for this purpose), and a method to sign-out a user
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager) : base(userManager, authenticationManager)
        {
        }
    }
}
