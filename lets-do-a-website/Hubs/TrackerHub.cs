using lets_do_a_website.Data;
using lets_do_a_website.Data.Entities;
using lets_do_a_website.Models;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;

namespace lets_do_a_website.Hubs
{
    public class TrackerHub:Hub
    {

        private readonly ITrackerData _trackerData;
        private readonly WTDRepo _repo;
        public TrackerHub(ITrackerData trackerData, WTDRepo repo)
        {
            _trackerData = trackerData;
            _repo = repo;
        } 

        public async Task NotifyDeath(TrackerNotify tracker)
        {
            var t = _trackerData.GetById(tracker.TrackerId);
            if (t == null) { return; }
            var d = t.DeathWays!.GetValueOrDefault(tracker.DeathId, null);
            if (d == null) { return; }
            d.Active = false;
            t.LastUsed= DateTime.UtcNow;
            tracker.DeathCount = t.DeathCount();

            await Clients.OthersInGroup($"tracker-{tracker.TrackerId}").SendAsync("ReceiveNewDeath", tracker);
    
            var m = _repo.GetMatchById(_repo.GetUserSettings(tracker.TrackerId).MatchId);

            if (m != null)
            {
                foreach(var entry in m.Entries)
                {
                    if (entry.Name.Equals(tracker.TrackerId))
                        continue;

                    await Clients.Group($"tracker-{entry.Name}").SendAsync("CounterUpdate", tracker);
                }
            }


        }
        public async Task NotifyUndo(TrackerNotify tracker)
        {
            var t = _trackerData.GetById(tracker.TrackerId);
            if (t == null) { return; }
            var d = t.DeathWays!.GetValueOrDefault(tracker.DeathId, null);
            if (d == null) { return; }
            d.Active = true;
            t.LastUsed = DateTime.UtcNow;

            await Clients.OthersInGroup($"tracker-{tracker.TrackerId}").SendAsync("ReceiveUndo", tracker);
        }

        public async Task NotifyRefresh(TrackerNotify Tracker)
        {
            await Clients.OthersInGroup($"tracker-{Tracker.TrackerId}").SendAsync("ForceRefresh", Tracker);
        }

        public async Task NotifyRandom(TrackerNotify tracker, string player)
        {
            var t = _trackerData.GetById(tracker.TrackerId);
            if (t == null) { return; }

            var d = t.DeathWays!.GetValueOrDefault(tracker.DeathId, null);
            if (d == null) { return; }
            d.Active = false;
            t.LastUsed = DateTime.UtcNow;

            await Clients.OthersInGroup($"tracker-{tracker.TrackerId}").SendAsync("ReceiveNewDeath", tracker);
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
                    if (entry.Name.Equals(trackerId) || entry.Status != 3)
                        continue;
                    var tracker = new TrackerNotify() { TrackerId = entry.Name, DeathId = 0, DeathCount = _trackerData.GetById(entry.Name).DeathCount() };
                    await Clients.Group($"tracker-{trackerId}").SendAsync("CounterUpdate", tracker);
                }
            }
            return;
        }
    }
}
