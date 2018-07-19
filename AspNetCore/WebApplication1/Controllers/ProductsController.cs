using Microsoft.AspNetCore.Mvc;

namespace MovieStore.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}