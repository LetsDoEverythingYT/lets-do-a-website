using System.ComponentModel.DataAnnotations;

namespace lets_do_a_website.Data.Entities
{
    public class UserSettings
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int OverlayMatchScores { get; set; } = 1;
        public int OverlayOnDeath { get; set; } = 1;
        public int TrackerOnDeath { get; set; } = 1; 
        public string ProfileImage { get; set; }
        public bool DarkMode { get; set; }

        public int MatchId { get; set; }
        public UserSettings(string user)
        {
            Name = user;
            OverlayMatchScores = 1;
            OverlayOnDeath = 1;
            TrackerOnDeath = 1;
            DarkMode = true;
            ProfileImage = "none";
            MatchId = 0;
        }

        public UserSettings()
        {
            Name = "0";
            OverlayMatchScores = 1;
            OverlayOnDeath = 1;
            TrackerOnDeath = 1;
            DarkMode = false;
            ProfileImage = "none";
            MatchId = 0;
        }
    }
}
