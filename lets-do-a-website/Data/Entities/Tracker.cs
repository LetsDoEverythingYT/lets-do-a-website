using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lets_do_a_website.Data.Entities
{
    public class Tracker
    {
        [Key]
        public int key { get; set; }    
        public string? Id { get; set; }
        public DateTime LastUsed { get; set; }

        [NotMapped]
       public Dictionary<int,DeathWay>? DeathWays { get; set; } 

        public Tracker() { }

        public Tracker(Tracker orig) 
        {
            Id = orig.Id;
            LastUsed = DateTime.UtcNow;
        
            DeathWays = new Dictionary<int, DeathWay>();
            foreach(var d in orig.DeathWays!.Values)
            {
                DeathWays.Add(d.Id,new DeathWay(d));
            }
        }

        public int DeathCount()
        {

            var deaths = "";
            var deathCount = 0;
            foreach(var d in DeathWays!.Values)
    {
                if (d.Active)
                {
                    deaths += "1";
                }
                else
                {
                    deaths += "0";
                    deathCount++;
                }

            }

            return DeathWays.Where( d => d.Value.Active == false).Count();
        }
    }
}
