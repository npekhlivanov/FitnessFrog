using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SignalRWithAuthentication.Data;

namespace SignalRWithAuthentication.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class DefaultController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration config;

        public DefaultController(SignInManager<ApplicationUser> signInManager, IConfiguration config)
        {
            this.signInManager = signInManager;
            this.config = config;
        }

        [HttpGet("/api/default")]
        public IActionResult GetResult()
        {
            return new JsonResult("success");
        }
    }
}