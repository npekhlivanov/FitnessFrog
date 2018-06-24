using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Treehouse.FitnessFrog.Shared.Models;
using Treehouse.FitnessFrog.Shared.Security;
using Treehouse.FitnessFrog.ViewModels;

namespace Treehouse.FitnessFrog.Controllers
{
    //[Authorize]
    public class AccountController : Controller
    {
        private readonly ApplicationUserManager _userManager;
        private readonly ApplicationSignInManager _signInManager;
        private readonly IAuthenticationManager _authenticationManager;

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, IAuthenticationManager authenticationManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authenticationManager = authenticationManager;
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(AccountRegisterViewModel viewModel)
        {
            while (ModelState.IsValid)
            {
                // Check if the email address is already used
                var existingUser = await _userManager.FindByEmailAsync(viewModel.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", $"The provided email address \"{viewModel.Email}\" has already been used to register an account.");
                    break;
                }

                // Instantiate a User object
                var user = new User
                {
                    Email = viewModel.Email,
                    UserName = viewModel.Email
                };

                // Create the user
                var result = await _userManager.CreateAsync(user, viewModel.Password);
                // If the user was successfully created...
                if (result.Succeeded)
                {
                    // Sign-in the user and redirect them to the web app's "Home" page
                    await _signInManager.SignInAsync(user,
                        isPersistent: false, // whether the authentication cookie should persist after the user's browser has been closed
                        rememberBrowser: false); // only used if you have two-factor authentication enabled

                    return RedirectToAction("Index", "Entries");
                }

                // If there were errors...
                foreach (var error in result.Errors)
                {
                    // Add model errors 
                    ModelState.AddModelError("", error);
                }
                break;
            }

            return View();
        }

        [AllowAnonymous]
        public ActionResult SignIn()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> SignIn(AccountSignInViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(viewModel.Email, viewModel.Password, viewModel.RememberMe, shouldLockout: false);
                switch (result)
                {
                    case SignInStatus.Success:
                        {
                            return RedirectToAction("Index", "Entries");
                        }
                    case SignInStatus.Failure:
                        {
                            ModelState.AddModelError("", "Invalid email or password!");
                            break;
                        }
                    case SignInStatus.LockedOut:
                        {
                            throw new NotImplementedException();
                        }
                    default:
                        {
                            throw new ApplicationException("Unexpected Microsoft.AspNet.Identity.Owin.SignInStatus enum value: " + result);
                        }

                }
            }

            return View(viewModel);
            
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult SignOut()
        {
            _authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Entries");
        }

    }
}