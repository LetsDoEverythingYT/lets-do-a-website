﻿namespace lets_do_a_website.Models
{
    public class TrackerNotify
    {
        public string TrackerId { get; set; }  
        public int DeathId { get; set; }

        public TrackerNotify()
        {
            TrackerId = "";
            DeathId = 0;
        }
    }
}