using System.ComponentModel.DataAnnotations;

namespace lets_do_a_website.Data.Entities
{
    public class Permissions
    {
        public int Id { get; set; }
        public string Streamer { get; set; }
        public string Mod { get; set; } 
    }
}
