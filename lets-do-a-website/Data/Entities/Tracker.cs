//using lets_do_a_website.Migrations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lets_do_a_website.Data.Entities
{
    public class Tracker
    {
        [Key]
        public int key { get; set; }
        public string Id { get; set; } = "default";
        public DateTime FirstUsed { get; set; }
        public DateTime LastUsed { get; set; }

        public string DataBits { get; set; } = "";

        [NotMapped]
       public Dictionary<int,DeathWay>? DeathWays { get; set; } 

        public Tracker() { }

        public Tracker(Tracker orig) 
        {
            Id = orig.Id;
            FirstUsed = DateTime.UtcNow;
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
            if(DeathCount() == 1)
                FirstUsed= DateTime.UtcNow;
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

        public int GetRandomDeath()
        {
            Random random = new Random();
            int charCount = DataBits.Length - DataBits.Replace("0", "").Length;
            int randomOccurence = random.Next(charCount) + 1;

            int count = 0;
            int index = -1;
            for (int i = 0; i < DataBits.Length; i++)
            {
                if (DataBits[i] == '0')
                {
                    count++;
                }

                if (count == randomOccurence)
                {
                    index = i;
                    break;
                }
            }

            return index;
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
