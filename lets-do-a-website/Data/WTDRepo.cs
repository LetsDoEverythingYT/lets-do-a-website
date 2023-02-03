using lets_do_a_website.Data.Entities;
using Org.BouncyCastle.Bcpg.OpenPgp;
using System.Linq;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace lets_do_a_website.Data
{
    public class WTDRepo
    {
        private readonly WTDContext _ctx;
        private readonly Tracker defaultTracker;

        public WTDRepo(WTDContext ctx)
        {
            _ctx = ctx;

            var filePath = "Data/ways.json";

            var json = System.IO.File.ReadAllText(filePath);

            var ways = JsonSerializer.Deserialize<IEnumerable<DeathWay>>(json)!.ToList();

            var deathMap = new Dictionary<int, DeathWay>();
            foreach (var way in ways)
            {
                deathMap.Add(way.Id, way);
            }

            defaultTracker = new Tracker
            {
                Id = "default",
                DeathWays = deathMap,
                //DataBits = new string('1', ways.Count())
                DataBits = new string('1', 100) //Forgot I have empty spaces in my ID map lol
            };
        }

        public IEnumerable<Permissions> GetAllMods(string streamer)
        {
            return _ctx.Permissions.Where(p => p.Streamer == streamer).ToList();
        }
        public bool DoesPermissionExist(Permissions p)
        {
            return DoesPermissionExist(p.Streamer, p.Mod);
        }
        public bool DoesPermissionExist(string streamer, string mod)
        {
            return (_ctx.Permissions.Where(perm => perm.Streamer.Equals(streamer) && perm.Mod.Equals(mod)).Any() );

        }
        public void AddPermission(Permissions p)
        {
            if (DoesPermissionExist(p))
                return;

            _ctx.Permissions.Add(p);
        }
        public void RemovePermissions(string streamer)
        {
            foreach(Permissions p in _ctx.Permissions.Where( p => !p.Mod.Equals(streamer))) {
               _ctx.Permissions.Remove(p);
            }
        }

        public IEnumerable<Permissions> GetAllStreamers(string mod)
        {
            return _ctx.Permissions.Where(p => p.Mod == mod).ToList();
        }

        public UserSettings GetUserSettings(string streamer)
        {
            return _ctx.UserSettings.Where( u => u.Name.Equals(streamer)).FirstOrDefault();
        }        
        public UserSettings GetOrAddUserSettings(string streamer)
        {
            var u = _ctx.UserSettings.Where(u => u.Name.Equals(streamer)).FirstOrDefault();
            if (u == null)
            {
                u = AddUserSettings(streamer);
            }
            return u;
        }
        public UserSettings AddUserSettings(string streamer)
        {
            var u = new UserSettings(streamer);
            _ctx.UserSettings.Add(u);  
            return u;
        }

        public Tracker AddTracker(string id)
        {
            Tracker t = new Tracker(defaultTracker);
            t.Id = id;
            _ctx.Trackers.Add(t);
            return t;
        }
        public Tracker GetTracker(string id, bool createIfDNE=false, bool buildDeathWays=false)
        {
            var t = _ctx.Trackers.Where(x => x.Id.Equals(id)).FirstOrDefault();
            if (t == null)
            {
                t = new Tracker(defaultTracker);
                t.Id = id;

                if (createIfDNE)
                {
                    AddTracker(id);
                }
            } else if(buildDeathWays)
            {
                t.RefreshDeaths(defaultTracker);
            }
            return t;
        }
        public IEnumerable<Tracker> GetAllTrackers()
        {
            return from t in _ctx.Trackers select t;
        }

        public void RemoveTracker(string id)
        {
            _ctx.Trackers.Remove(GetTracker(id));
        }




        public Match AddMatch(string streamer)
        {
            if (GetMatch(streamer) is Match m) 
                return m;

            m = new Match(streamer);
            _ctx.Matches.Add(m);
            return m;
        }

        public Match? GetMatch(string streamer)
        {
            //Why do I have to manually load the entries? No idea. without it it wasn't doing a db call
            //https://learn.microsoft.com/en-us/ef/ef6/querying/related-data
            var m = _ctx.Matches.Where(m => m.Streamer == streamer).FirstOrDefault();
            if (m == null) return null;
            _ctx.Entry(m).Collection("Entries").Load();
            return m;
        }
        public Match? GetMatchById(int id)
        {
            //Why do I have to manually load the entries? No idea. without it it wasn't doing a db call
            //https://learn.microsoft.com/en-us/ef/ef6/querying/related-data
            var m = _ctx.Matches.Where(m => m.Id == id).FirstOrDefault();
            if (m == null) return null;
            _ctx.Entry(m).Collection("Entries").Load();
            return m;
        }

        public void DeleteMatch(string streamer)
        {
            foreach(var i in _ctx.Invites.Where( i => i.HostStreamer.Equals(streamer)))
            {
                _ctx.Invites.Remove(i);
            }
            _ctx.Matches.Remove(GetMatch(streamer)!);
        }
        public void AddInvite(int matchId, string hostStreamer, string guestStreamer) 
        {
            if (GetInvite(hostStreamer, guestStreamer) == null)
            {
                _ctx.Invites.Add(new Invite(matchId, hostStreamer, guestStreamer));
            }
        }

        public Invite? GetInvite(string hostStreamer, string guestStreamer)
        {
            return _ctx.Invites.Where(i => i.HostStreamer.Equals(hostStreamer) && i.GuestStreamer.Equals(guestStreamer)).FirstOrDefault();
        }
        public Invite? GetInviteByMatchId(int matchId, string guestStreamer)
        {
            return _ctx.Invites.Where(i => i.MatchId == matchId && i.GuestStreamer.Equals(guestStreamer)).FirstOrDefault();
        }
        public void DeleteInvite(int matchId, string hostStreamer, string guestStreamer)
        {
            if (GetInvite(hostStreamer, guestStreamer) is Invite i) _ctx.Invites.Remove(i);
        }
        public void DeleteAllInvites(int matchId)
        {
            foreach (var i in _ctx.Invites.Where(i => i.MatchId == matchId))
                _ctx.Invites.Remove(i);
        }
        public void DeleteInvite(Invite invite)
        {
            _ctx.Invites.Remove(invite);
        }

        public List<Invite> GetAllInvites(string guestStreamer)
        {
            return _ctx.Invites.Where(i => i.GuestStreamer.Equals(guestStreamer)).ToList();
        }


        public void AddRunStats(RunStats stat)
        {
            _ctx.RunStats.Add(stat);
        }
        public bool SaveAll()
        {
            return _ctx.SaveChanges() > 0;
        }
    }
}
