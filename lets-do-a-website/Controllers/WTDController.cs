using lets_do_a_website.Data;
using lets_do_a_website.Data.Entities;
using lets_do_a_website.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace lets_do_a_website.Controllers
{
    public class WTDController : Controller
    {
        private readonly ITrackerData _trackerData;
        private readonly WTDRepo _repo;

        public WTDController(ITrackerData trackerData, WTDRepo repo)
        {
            _trackerData = trackerData;
            _repo = repo;
        }


        [Authorize]
        public IActionResult Index()
        {
            ViewData["settings"] = _repo.GetOrAddUserSettings(User.Identity!.Name!);
            ViewData["streamers"] = _repo.GetAllStreamers(User.Identity!.Name!);
            ViewData["mods"] = _repo.GetAllMods(User.Identity!.Name!);
         
            return View();
        }

        [Authorize]
        public IActionResult Tracker(string id)
        {
            if(!id.Equals(User.Identity!.Name))
            { 
                if(!_repo.DoesPermissionExist(id, User.Identity!.Name!))
                    return RedirectToAction("Index");
            }

            return View(_trackerData.GetById(id, true));
        }

        public IActionResult Overlay(string id)
        {
            var u = _repo.GetUserSettings(id);
            if(u == null)
            {
                ViewData["img"] = "none";
            }
            else
            {
                ViewData["img"] = u.ProfileImage;
            }
            return PartialView("Overlay",_trackerData.GetById(id, false));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public IActionResult Reset(string id)
        {
            if (!id.Equals(User.Identity!.Name))
            {
                if (!_repo.DoesPermissionExist(id, User.Identity!.Name!))
                    return RedirectToAction("Index");
            }

            _trackerData.RemoveTracker(id);
            _trackerData.AddTracker(id);
            return RedirectToAction("Tracker", new { id });
        }

        [Authorize]
        public IActionResult AddMod(string id)
        {
            if (id != null)
            {
                _repo.AddPermission(new Permissions { Streamer = User.Identity!.Name!, Mod = id });
                _repo.SaveAll();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Hyduron()
        {
            return View();
        }
    }
}