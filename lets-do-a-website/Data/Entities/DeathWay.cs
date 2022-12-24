using System.Collections.Generic;

namespace lets_do_a_website.Data.Entities
{
    public class DeathWay : ICloneable 
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public string Category { get; set; }


        public  DeathWay ()
        {
            Id = -1;
            Name = "null";
            Category = "null";
            Active = true;  
        }

        public DeathWay(DeathWay orig)
        {
            Id = orig.Id;
            Name= orig.Name;
            Category = orig.Category;
            Active = orig.Active;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
