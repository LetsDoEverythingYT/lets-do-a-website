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

            _repo.SaveAll();

            await Clients.Group($"tracker-{notification.TrackerId}").SendAsync("ReceiveUndo", notification);

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

        public async Task ForceUndo(TrackerNotify notification)
        {
            var tracker = _repo.GetTracker(notification.ForcedFrom);
            if (tracker == null) { return; }
            var match = _repo.GetMatchById(_repo.GetUserSettings(notification.ForcedFrom).MatchId);
            if (match == null) { return; }

            if(notification.TrackerId.Equals("-1"))
            {
                notification.TrackerId = match.GetRandomParticipant();
            }

            var target = _repo.GetTracker(notification.TrackerId);
            if (target == null) { return; }
            var p = match.getParticipant(notification.TrackerId);
            if(p == null || p.Status != 3) { return; }


            var id = target.GetRandomDeath();
            if (id == -1) { return; }

            notification.DeathId= id;

            await NotifyUndo(notification);

            await Clients.Group($"tracker-{notification.TrackerId}").SendAsync("ForceComplete", notification);
            await Clients.Group($"tracker-{notification.ForcedFrom}").SendAsync("ForceComplete", notification);

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
