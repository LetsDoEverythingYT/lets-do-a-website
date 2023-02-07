using lets_do_a_website.Data;
using lets_do_a_website.Data.Entities;
using lets_do_a_website.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace lets_do_a_website.Controllers
{
    public class WTDController : Controller
    {
        private readonly WTDRepo _repo;

        public WTDController(WTDRepo repo)
        {
            _repo = repo;
        }
        
        public IActionResult Index(string returnUrl = "/WTD")
        {
            if(User.Identity!.Name == null)
            {
                return View("IndexNotLoggedin", new LoginModel { ReturnUrl = returnUrl });
            }

            ViewData["settings"] = _repo.GetOrAddUserSettings(User.Identity!.Name!);
            ViewData["streamers"] = _repo.GetAllStreamers(User.Identity!.Name!);
            ViewData["mods"] = _repo.GetAllMods(User.Identity!.Name!);
         
            return View();
        }

        [Authorize]
        public IActionResult Tracker(string id)
        {
            if(id is null)
                return RedirectToAction("Index");

            if (!id.Equals(User.Identity!.Name))
            { 
                if(!_repo.DoesPermissionExist(id, User.Identity!.Name!))
                    return RedirectToAction("Index");
            }


            var settings = _repo.GetOrAddUserSettings(id);
            ViewData["settings"] = settings;
            ViewData["match"] = _repo.GetMatchById(settings.MatchId);
            ViewData["invites"] = _repo.GetAllInvites(id);
            var t = _repo.GetTracker(id, true, true);
            _repo.SaveAll();
            return View(t);
        }

        public IActionResult Overlay(string id, string id2)
        {
            var u = _repo.GetUserSettings(id);
            if(u == null)
            {
                u = new UserSettings();
            }
            ViewData["settings"] = u;
            ViewData["match"] = _repo.GetMatchById(u.MatchId);
            var tracker = _repo.GetTracker(id, false, true);

            if (id2 == "2")
                return PartialView("MonopolyOverlay", tracker);
            if (id2 == "3")
                return PartialView("MatchOnlyOverlay", tracker);

            return PartialView("Overlay", tracker);

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

            //save partial run. full runs are saved at 50 in the hub
            var t = _repo.GetTracker(id);
            var deaths = t.DeathCount();
            if(deaths > 10 && deaths < 50) {
                _repo.SaveRun(t);
            }

            _repo.RemoveTracker(id);
            _repo.AddTracker(id);
            _repo.SaveAll();

            return RedirectToAction("Tracker", new { id });
        }

        [Authorize]
        public IActionResult AddMod(string id)
        {
            if (id != null)
            {
                _repo.AddPermission(new Permissions { Streamer = User.Identity!.Name!, Mod = id.ToLower().Trim() });
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
        public IActionResult ChangeSettings(string id, int tracker, int overlay, int overlayMatch)
        {
            if (!id.Equals(User.Identity!.Name))
            {
                if (!_repo.DoesPermissionExist(id, User.Identity!.Name!))
                    return RedirectToAction("Index");
            }

            //Apply to steamer stats, because no one is using mod feature
            //but in the future, tracker option should be at user/mod level, and 
            //overlay should be at streamer level

            //Is this needed? V Not really, be it reminds me I'm using these magic numbers to fix later
            if(tracker <0 || tracker > 5 || overlay <0 || overlay > 3 || overlayMatch < 1 || overlayMatch > 2)
                return RedirectToAction("Index");

            var u = _repo.GetUserSettings(id);
            u.TrackerOnDeath = tracker;
            u.OverlayOnDeath = overlay;
            u.OverlayMatchScores= overlayMatch;

            _repo.SaveAll();

            return RedirectToAction("Tracker", new { id });
        }


        [Authorize]
        public IActionResult EditMatch(string hostStreamer, string guestStreamer, string action2)
        {
            if (!hostStreamer.Equals(User.Identity!.Name))
            {
                if (!_repo.DoesPermissionExist(hostStreamer, User.Identity!.Name!))
                    return RedirectToAction("Index");
            }

            if (hostStreamer is null || guestStreamer is null)
                return RedirectToAction("Index");

            hostStreamer = hostStreamer.ToLower().Trim();
            guestStreamer = guestStreamer.ToLower().Trim();

            var settings = _repo.GetUserSettings(hostStreamer);

            var match = _repo.GetMatch(hostStreamer);
            if (match == null)
            {
                match = _repo.AddMatch(hostStreamer);
                match.AddParticipant(hostStreamer, 3, _repo.GetUserSettings(hostStreamer).ProfileImage);
                _repo.SaveAll();

                settings.MatchId = match.Id;
            }

            if (action2.Equals("add"))
            {
                _repo.AddInvite(match.Id, hostStreamer, guestStreamer);
                var guest = _repo.GetUserSettings(guestStreamer);
                var pic = guest is not null ? guest.ProfileImage : "none";
                match.AddParticipant(guestStreamer, 1, pic);
            }
            if (action2.Equals("remove") && !guestStreamer.Equals(hostStreamer))
            {
                _repo.DeleteInvite(match.Id, hostStreamer, guestStreamer);
                if(match.getParticipant(guestStreamer).Status==3)
                {
                    _repo.GetUserSettings(guestStreamer).MatchId=0;
                }
                match.RemoveParticipant(guestStreamer);
            }
            if (action2.Equals("delete"))
            {
                _repo.DeleteAllInvites(match.Id);
                foreach(var e in match.Entries)
                {
                    if(e.Status== 3)
                        _repo.GetUserSettings(e.Name).MatchId = 0;
                }
                _repo.DeleteMatch(hostStreamer);
                settings.MatchId = 0;
            }


            _repo.SaveAll();
            return RedirectToAction("Tracker", new { id = hostStreamer });
        }

        [Authorize]
        public IActionResult LeaveMatch(string guest)
        {
            if (!guest.Equals(User.Identity!.Name))
            {
                if (!_repo.DoesPermissionExist(guest, User.Identity!.Name!))
                    return RedirectToAction("Index");
            }

            var settings = _repo.GetUserSettings(guest);

            var match = _repo.GetMatchById(settings.MatchId);

            if (match is null || guest is null)
                return RedirectToAction("Index");

            settings.MatchId = 0;
            match.getParticipant(guest).Status = 2;

            _repo.SaveAll();
            return RedirectToAction("Tracker", new { id = guest });
        }


        [Authorize]
        public IActionResult AcceptInvite(int matchId, string guestStreamer)
        {
            if (!guestStreamer.Equals(User.Identity!.Name))
            {
                if (!_repo.DoesPermissionExist(guestStreamer, User.Identity!.Name!))
                    return RedirectToAction("Index");
            }

            if (guestStreamer is null)
                return RedirectToAction("Index");

            guestStreamer = guestStreamer.ToLower().Trim();

            var invite = _repo.GetInviteByMatchId(matchId, guestStreamer);

            if (invite is null)
                return RedirectToAction("Tracker", new { id = guestStreamer });

            var settings = _repo.GetUserSettings(guestStreamer);

            if (settings.MatchId != 0)
                return RedirectToAction("Tracker", new { id = guestStreamer });


            settings.MatchId = matchId;
            _repo.DeleteInvite(invite);
            var match = _repo.GetMatchById(matchId);
            match.getParticipant(guestStreamer).Status=3;


            _repo.SaveAll();
            return RedirectToAction("Tracker", new { id = guestStreamer });
        }

        [Authorize]
        public IActionResult DeclineInvite(int matchId, string guestStreamer)
        {
            if (!guestStreamer.Equals(User.Identity!.Name))
            {
                if (!_repo.DoesPermissionExist(guestStreamer, User.Identity!.Name!))
                    return RedirectToAction("Index");
            }

            if (guestStreamer is null)
                return RedirectToAction("Index");

            guestStreamer = guestStreamer.ToLower().Trim();

            var invite = _repo.GetInviteByMatchId(matchId, guestStreamer);

            if (invite is null)
                return RedirectToAction("Tracker", new { id = guestStreamer });


            _repo.DeleteInvite(invite);
            var match = _repo.GetMatchById(matchId);
            match.getParticipant(guestStreamer).Status = 2;

            _repo.SaveAll();
            return RedirectToAction("Tracker", new { id = guestStreamer });
        }




        public IActionResult Hyduron()
        {
            return View();
        }
    }
}