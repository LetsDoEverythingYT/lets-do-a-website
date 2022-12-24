using lets_do_a_website.Data.Entities;

using System.Text.Json;

namespace lets_do_a_website.Data
{
    public interface ITrackerData
    {
        Tracker AddTracker(string id);
        void RemoveTracker(string id);
        Tracker GetById(string id, bool createIfDNE = false);
        IEnumerable<Tracker> GetAll();

    }

    public class InMemoryTrackerData : ITrackerData
    {
        private readonly List<Tracker> trackers; 

        private readonly Tracker defaultTracker;
        public InMemoryTrackerData() 
        {
            trackers = new List<Tracker>();
            //This should probably be a Dictionary as well, not iterating over it just keyed reads
            
            //            IWebHostEnvironment env = null;
            //            var filePath = Path.Combine(env.ContentRootPath, "Data/ways.json");


            var filePath = "Data/ways.json";

            var json = System.IO.File.ReadAllText(filePath);
            
            var ways = JsonSerializer.Deserialize<IEnumerable<DeathWay>>(json)!.ToList();

            var deathMap = new Dictionary<int, DeathWay>();
            foreach(var way in ways)
            {
                deathMap.Add(way.Id, way);
            }

            defaultTracker = new Tracker
            {
                Id = "default",
                DeathWays = deathMap
            };
        }

        public Tracker AddTracker(string id)
        {
            Tracker t = new Tracker(defaultTracker);

            t.Id = id;
            trackers.Add(t);
            return t;
        }
        public void RemoveTracker(string id)
        {
            trackers.RemoveAt(trackers.FindIndex(t => t.Id == id)); 
        }

        public IEnumerable<Tracker> GetAll()
        {
            return from t in trackers
                   select t;
        }

        public Tracker GetById(string id, bool createIfDNE)
        {
            var t = trackers.SingleOrDefault(x => x!.Id!.Equals(id), null);
            if(t == null)
            {
                t = new Tracker(defaultTracker);
                t.Id = id;

                if (createIfDNE)
                {
                    AddTracker(id);
                }

            }
            return t;
        }
    }
}
