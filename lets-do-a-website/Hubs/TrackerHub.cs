using lets_do_a_website.Data;
using lets_do_a_website.Data.Entities;
using lets_do_a_website.Models;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;

namespace lets_do_a_website.Hubs
{
    public class TrackerHub:Hub
    {

        private readonly WTDRepo _repo;
        public TrackerHub(WTDRepo repo)
        {
            _repo = repo;
        } 

        public async Task NotifyDeath(TrackerNotify notification)
        {
            var tracker = _repo.GetTracker(notification.TrackerId);
            if (tracker == null) { return; }

            tracker.addDeath(notification.DeathId);
            notification.DeathCount = tracker.DeathCount();

            _repo.SaveAll();

            await Clients.OthersInGroup($"tracker-{notification.TrackerId}").SendAsync("ReceiveNewDeath", notification);
            
            var m = _repo.GetMatchById(_repo.GetUserSettings(notification.TrackerId).MatchId);

            if (m != null)
            {
                foreach(var entry in m.Entries)
                {
                    await Clients.Group($"tracker-{entry.Name}").SendAsync("CounterUpdate", notification);
                }
            }


        }
        public async Task NotifyUndo(TrackerNotify notification)
        {
            var tracker = _repo.GetTracker(notification.TrackerId);
            if (tracker == null) { return; }

            tracker.removeDeath(notification.DeathId);
            notification.DeathCount = tracker.DeathCount();

            await Clients.OthersInGroup($"tracker-{notification.TrackerId}").SendAsync("ReceiveUndo", notification);

            var m = _repo.GetMatchById(_repo.GetUserSettings(notification.TrackerId).MatchId);

            if (m != null)
            {
                foreach (var entry in m.Entries)
                {
                    await Clients.Group($"tracker-{entry.Name}").SendAsync("CounterUpdate", notification);
                }
            }
        }

        public async Task NotifyRefresh(TrackerNotify notification)
        {
            await Clients.OthersInGroup($"tracker-{notification.TrackerId}").SendAsync("ForceRefresh", notification);
        }

        public async Task NotifyRandom(TrackerNotify notification, string player)
        {
            var t = _repo.GetTracker(notification.TrackerId);
            if (t == null) { return; }

            var d = t.DeathWays!.GetValueOrDefault(notification.DeathId, null);
            if (d == null) { return; }
            d.Active = false;
            t.LastUsed = DateTime.UtcNow;

            await Clients.OthersInGroup($"tracker-{notification.TrackerId}").SendAsync("ReceiveNewDeath", notification);
        }

        public override async Task OnConnectedAsync()
        {
            var trackerId = Context.GetHttpContext()!.Request.Query["tracker"];
            await Groups.AddToGroupAsync(Context.ConnectionId, $"tracker-{trackerId}");
            await base.OnConnectedAsync();

            //can I add calls to initialize opponent scores here? prolly ya

            var m = _repo.GetMatchById(_repo.GetUserSettings(trackerId).MatchId);
            if (m != null)
            {
                foreach (var entry in m.Entries)
                {
                    if (entry.Status != 3)
                        continue;
                    var tracker = new TrackerNotify() { TrackerId = entry.Name, DeathId = 0, DeathCount = _repo.GetTracker(entry.Name).DeathCount() };
                    await Clients.Group($"tracker-{trackerId}").SendAsync("CounterUpdate", tracker);
                }
            }
            return;
        }
    }
}
