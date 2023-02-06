namespace lets_do_a_website.Models
{
    public class TrackerNotify
    {
        public string TrackerId { get; set; }  
        public int DeathId { get; set; }
        public int DeathCount { get; set; }
        public string ForcedFrom { get; set; } = "";

        public TrackerNotify()
        {
            TrackerId = "";
            DeathId = 0;
            DeathCount = 0;
            ForcedFrom = "";
        }
    }
}