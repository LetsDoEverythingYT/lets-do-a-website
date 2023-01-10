using System.ComponentModel.DataAnnotations;

namespace lets_do_a_website.Data.Entities
{
    public class UserSettings
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int OverlayOnDeath { get; set; }
        public  int TrackerOnDeath { get; set; }
        public string ProfileImage { get; set; }
        public bool DarkMode { get; set; }
        public UserSettings(string user)
        {
            Name = user;
            OverlayOnDeath = 1;
            TrackerOnDeath = 1;
            DarkMode = true;
            ProfileImage = "none";
        }

        public UserSettings()
        {
            Name = "0"; 
            OverlayOnDeath = 1;
            TrackerOnDeath = 1;
            DarkMode = false;
            ProfileImage = "none";
        }
    }
}
