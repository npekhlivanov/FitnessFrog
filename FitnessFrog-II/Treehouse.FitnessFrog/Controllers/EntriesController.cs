using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Treehouse.FitnessFrog.Shared.Data;
using Treehouse.FitnessFrog.Shared.Models;
using Treehouse.FitnessFrog.ViewModels;

namespace Treehouse.FitnessFrog.Controllers
{
    public class EntriesController : Controller
    {
        private EntriesRepository _entriesRepository = null;
        private ActivitiesRepository _activitiesRepository = null;

        public EntriesController(EntriesRepository entriesRepository, ActivitiesRepository activitiesRepository)
        {
            _entriesRepository = entriesRepository;
            _activitiesRepository = activitiesRepository;
        }

        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            IList<Entry> entries = _entriesRepository.GetList(userId);

            // Calculate the total activity.
            decimal totalActivity = entries
                .Where(e => e.Exclude == false)
                .Sum(e => e.Duration);

            // Determine the number of days that have entries.
            int numberOfActiveDays = entries
                .Select(e => e.Date)
                .Distinct()
                .Count();

            var viewModel = new EntriesIndexViewModel()
            {
                Entries = entries,
                TotalActivity = totalActivity,
                AverageDailyActivity = numberOfActiveDays != 0 ?
                    (totalActivity / numberOfActiveDays) : 0
            };

            return View(viewModel);
        }

        public ActionResult Add()
        {
            var viewModel = new EntriesAddViewModel();
            viewModel.Entry.UserId = User.Identity.GetUserId();

            viewModel.Init(_activitiesRepository);

            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Add(EntriesAddViewModel viewModel)
        {
            ValidateEntry(viewModel.Entry);

            if (ModelState.IsValid)
            {
                var entry = viewModel.Entry;
                var userId = User.Identity.GetUserId();
                entry.UserId = userId; // ensure the value from the browser is ignored and the current user id is used
                _entriesRepository.Add(entry);

                TempData["Message"] = "Your entry was successfully added!";

                return RedirectToAction("Index");
            }

            viewModel.Init(_activitiesRepository);

            return View(viewModel);
        }

        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userId = User.Identity.GetUserId();
            Entry entry = _entriesRepository.Get(id.Value, userId);

            if (entry == null)
            {
                return HttpNotFound();
            }

            var viewModel = new EntriesEditViewModel()
            {
                Entry = entry
            };
            viewModel.Init(_activitiesRepository);

            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(EntriesEditViewModel viewModel)
        {
            ValidateEntry(viewModel.Entry);

            if (ModelState.IsValid)
            {
                var entry = viewModel.Entry;
                var userId = User.Identity.GetUserId(); 
                if (!_entriesRepository.EntryIsOwnedByUser(viewModel.Id, userId))
                {
                    return HttpNotFound();
                }

                entry.UserId = userId; // override the userId with the actual one
                _entriesRepository.Update(entry);

                TempData["Message"] = "Your entry was successfully updated!";

                return RedirectToAction("Index");
            }

            viewModel.Init(_activitiesRepository);

            return View(viewModel);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userId = User.Identity.GetUserId();
            Entry entry = _entriesRepository.Get((int)id, userId);

            if (entry == null)
            {
                return HttpNotFound();
            }

            return View(entry);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var userId = User.Identity.GetUserId();
            if (!_entriesRepository.EntryIsOwnedByUser(id, userId))
            {
                return HttpNotFound();
            }

            _entriesRepository.Delete(id);

            TempData["Message"] = "Your entry was successfully deleted!";

            return RedirectToAction("Index");
        }

        private void ValidateEntry(Entry entry)
        {
            // If there aren't any "Duration" field validation errors
            // then make sure that the duration is greater than "0".
            if (ModelState.IsValidField("Duration") && entry.Duration <= 0)
            {
                ModelState.AddModelError("Duration",
                    "The Duration field value must be greater than '0'.");
            }
        }
    }
}