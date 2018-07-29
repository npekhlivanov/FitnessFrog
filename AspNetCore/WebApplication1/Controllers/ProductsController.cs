using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreTest.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            //var contacts = ContactsRepository.GetContacts();
            //var viewModel = new ProductIndexViewModel { Contacts = contacts, Count = contacts.Count };
            //return View(viewModel);
            return View();
        }
    }
}