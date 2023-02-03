using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lets_do_a_website.Data.Entities
{
    public class Tracker
    {
        [Key]
        public int key { get; set; }
        public string Id { get; set; } = "default";
        public DateTime LastUsed { get; set; }

        public string DataBits { get; set; } = "";

        [NotMapped]
       public Dictionary<int,DeathWay>? DeathWays { get; set; } 

        public Tracker() { }

        public Tracker(Tracker orig) 
        {
            Id = orig.Id;
            LastUsed = DateTime.UtcNow;
            DataBits = orig.DataBits;
        
            DeathWays = new Dictionary<int, DeathWay>();
            foreach(var d in orig.DeathWays!.Values)
            {
                DeathWays.Add(d.Id,new DeathWay(d));
            }
        }

        public int DeathCount()
        {
            return DataBits.Count( c => c =='0');
        }

        public void addDeath(int id)
        {
            char[] ch = DataBits.ToCharArray();
            if (id > ch.Length - 1 || id < 0)
                return;

            ch[id] = '0';
            DataBits = new string(ch);
            LastUsed = DateTime.UtcNow;
        }

        public void removeDeath(int id)
        {
            char[] ch = DataBits.ToCharArray();
            if (id > ch.Length - 1 || id < 0)
                return;

            ch[id] = '1';
            DataBits = new string(ch);
            LastUsed = DateTime.UtcNow;

        }

        //Required because the deathway junk isn't saved in the database
        public void RefreshDeaths(Tracker t)
        {
            DeathWays = new Dictionary<int, DeathWay>();
            char[] ch = DataBits.ToCharArray();

            foreach (var d in t.DeathWays!.Values)
            {
                var NewDeath = new DeathWay(d);
                if (ch[d.Id] == '0')
                {
                    NewDeath.Active = false;
                }
                DeathWays.Add(d.Id, NewDeath);
            }
        }
    }
}
