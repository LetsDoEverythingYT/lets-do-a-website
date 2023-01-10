using lets_do_a_website.Data.Entities;

namespace lets_do_a_website.Data
{
    public class WTDRepo
    {
        private readonly WTDContext _ctx;

        public WTDRepo(WTDContext ctx)
        {
            _ctx = ctx;
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
            return (_ctx.Permissions.Where(perm => perm.Streamer.Equals(streamer) && perm.Mod.Equals(mod)).Count() > 0);

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
