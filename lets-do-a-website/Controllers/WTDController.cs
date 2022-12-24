using lets_do_a_website.Data;
using lets_do_a_website.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace lets_do_a_website.Controllers
{
    public class WTDController : Controller
    {
        private readonly ITrackerData _trackerData;

        public WTDController(ITrackerData trackerData)
        {
            _trackerData = trackerData;
        }


        [Authorize]
        public IActionResult Index()
        {
            return View(_trackerData.GetById(User.Identity!.Name!, true));
        }

        [Authorize]
        public IActionResult Tracker(string id)
        {
            if(!id.Equals(User.Identity!.Name))
            { 
                return RedirectToAction("Tracker", new { id = User.Identity.Name });
            }

            return View(_trackerData.GetById(id, true));

        }

        public IActionResult Overlay(string id)
        {
            return PartialView(_trackerData.GetById(id, false));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public IActionResult Reset()
        {
            _trackerData.RemoveTracker(User.Identity!.Name!);
            _trackerData.AddTracker(User.Identity!.Name!);
            return RedirectToAction("Tracker", new {id = User.Identity.Name });
        }

        public IActionResult Hyduron()
        {
            return View();
        }
    }
}