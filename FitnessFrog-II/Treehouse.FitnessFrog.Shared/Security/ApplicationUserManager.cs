using Microsoft.AspNet.Identity;
using Treehouse.FitnessFrog.Shared.Models;

namespace Treehouse.FitnessFrog.Shared.Security
{
    /// <summary>
    /// Provide functionality for working with user accounts, including finding, creating, updating, and deleting user accounts, 
    /// adding a password to a user's account, changing a user's password, etc.
    /// </summary>
    public class ApplicationUserManager : UserManager<User>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="store">This interface is provided by the Identity Entity Framework, using the Context; 
        /// The user store instance will be provided by the DI container</param>
        public ApplicationUserManager(IUserStore<User> store) : base(store)   
        {
        }
    }
}
