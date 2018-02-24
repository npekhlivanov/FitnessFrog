using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Treehouse.FitnessFrog.Data;
using Treehouse.FitnessFrog.Models;

namespace Treehouse.FitnessFrog.Controllers
{
    public class EntriesController : Controller
    {
        private EntriesRepository _entriesRepository = null;

        public EntriesController()
        {
            _entriesRepository = new EntriesRepository();
        }

        public ActionResult Index()
        {
            List<Entry> entries = _entriesRepository.GetEntries();

            // Calculate the total activity.
            double totalActivity = entries
                .Where(e => e.Exclude == false)
                .Sum(e => e.Duration);

            // Determine the number of days that have entries.
            int numberOfActiveDays = entries
                .Select(e => e.Date)
                .Distinct()
                .Count();

            ViewBag.TotalActivity = totalActivity;
            ViewBag.AverageDailyActivity = (totalActivity / (double)numberOfActiveDays);

            return View(entries);
        }

        public ActionResult Add()
        {
            var entry = new Entry()
            {
                Date = DateTime.Today
            };

            SetupActivitiesSelectList();
            return View(entry);
        }

        //[ActionName("Add")] // may be used if the method name differs
        [HttpPost]
        public ActionResult Add(/*[Bind(Include = "ID,Title,ReleaseDate,Genre,Price,Rating")]*/ Entry entry)
        {
            ValidateEntry(entry);

            if (ModelState.IsValid)
            {
                _entriesRepository.AddEntry(entry);
                TempData["Message"] = "The entry has been saved.";
                return RedirectToAction("Index");
            }

            // DropDown list items do not automatically populate on a PostBack, so we need to fill them again
            SetupActivitiesSelectList();
            return View();
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var entry = _entriesRepository.GetEntry(id.Value);
            if (entry == null)
            {
                return HttpNotFound();
            }

            SetupActivitiesSelectList();
            return View(entry);
        }

        [HttpPost]
        public ActionResult Edit(Entry entry)
        {
            ValidateEntry(entry);

            if (ModelState.IsValid)
            {
                _entriesRepository.UpdateEntry(entry);
                TempData["Message"] = "The entry has been updated.";
                return RedirectToAction("Index");
            }

            SetupActivitiesSelectList();
            return View(entry);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var entry = _entriesRepository.GetEntry(id.Value);
            if (entry == null)
            {
                return HttpNotFound();
            }

            return View(entry);
        }

        [HttpPost]
        public ActionResult Delete (int id)
        {
            _entriesRepository.DeleteEntry(id);
            TempData["Message"] = "The entry has been deleted.";
            return RedirectToAction("Index");
        }

        private void ValidateEntry(Entry entry)
        {
            if (ModelState.IsValidField("Duration") && entry.Duration <= 0)
            {
                ModelState.AddModelError("Duration", "The Duraition field must be greater than zero.");
            }
        }

        private void SetupActivitiesSelectList()
        {
            ViewBag.ActivitySelectListItems = new SelectList(Data.Data.Activities, "Id", "Name");
        }
    }
}