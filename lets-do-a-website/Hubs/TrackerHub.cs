using lets_do_a_website.Data;
using lets_do_a_website.Data.Entities;
using lets_do_a_website.Models;
using Microsoft.AspNetCore.SignalR;

namespace lets_do_a_website.Hubs
{
    public class TrackerHub:Hub
    {

        private readonly ITrackerData _trackerData;
        public TrackerHub(ITrackerData trackerData)
        {
            _trackerData = trackerData;
        } 

        public async Task NotifyDeath(TrackerNotify tracker)
        {
            var t = _trackerData.GetById(tracker.TrackerId);
            if (t == null) { return; }
            var d = t.DeathWays!.GetValueOrDefault(tracker.DeathId, null);
            if (d == null) { return; }
            d.Active = false;
            t.LastUsed= DateTime.UtcNow;

            await Clients.OthersInGroup($"tracker-{tracker.TrackerId}").SendAsync("ReceiveNewDeath", tracker);
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


        public override async Task OnConnectedAsync()
        {
            var groupName = $"tracker-{Context.GetHttpContext()!.Request.Query["tracker"]}";
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await base.OnConnectedAsync();
            return;
        }
    }
}
