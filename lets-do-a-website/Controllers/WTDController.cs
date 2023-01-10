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


            ViewData["settings"] = _repo.GetOrAddUserSettings(id);
            return View(_trackerData.GetById(id, true));
        }

        public IActionResult Overlay(string id)
        {
            var u = _repo.GetUserSettings(id);
            if(u == null)
            {
                u = new UserSettings();
            }
            ViewData["settings"] = u;
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


            var rs = new RunStats(id, DateTime.UtcNow, _trackerData.Stringify(id));
            
            _repo.AddRunStats(rs);
            _repo.SaveAll();

            _trackerData.RemoveTracker(id);
            _trackerData.AddTracker(id);
            return RedirectToAction("Tracker", new { id });
        }

        [Authorize]
        public IActionResult AddMod(string id)
        {
            if (id != null)
            {
                _repo.AddPermission(new Permissions { Streamer = User.Identity!.Name!, Mod = id.ToLower() });
                _repo.SaveAll();
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult RemoveMods()
        {
            _repo.RemovePermissions(User.Identity!.Name);
            _repo.SaveAll();
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult ChangeSettings(string id, int tracker, int overlay)
        {
            if (!id.Equals(User.Identity!.Name))
            {
                if (!_repo.DoesPermissionExist(id, User.Identity!.Name!))
                    return RedirectToAction("Index");
            }

            //Apply to steamer stats, because no one is using mod feature
            //but in the future, tracker option should be at user/mod level, and 
            //overlay should be at streamer level

            if(tracker <0 || tracker > 5 || overlay <0 || overlay > 3)
                return RedirectToAction("Index");

            var u = _repo.GetUserSettings(id);
            u.TrackerOnDeath = tracker;
            u.OverlayOnDeath = overlay;
            _repo.SaveAll();

            return RedirectToAction("Tracker", new { id });
        }
        public IActionResult Hyduron()
        {
            return View();
        }
    }
}