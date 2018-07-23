using MovieStore.Data;
using Microsoft.AspNetCore.Mvc;
using MovieStore.ViewModels;

namespace MovieStore.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            var contacts = ContactsRepository.GetContacts();
            var viewModel = new ProductIndexViewModel { Contacts = contacts, Count = contacts.Count };
            return View(viewModel);
        }
    }
}