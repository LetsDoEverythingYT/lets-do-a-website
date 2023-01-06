using System.ComponentModel.DataAnnotations;

namespace lets_do_a_website.Data.Entities
{
    public class UserSettings
    {
        [Key]
        public string Id { get; set; }
        public bool DeleteOnDeath { get; set; }
        public string ProfileImage { get; set; }

        public UserSettings(string user)
        {
            Id = user;
            DeleteOnDeath = true;
            ProfileImage = "none";
        }

        public UserSettings()
        {
            Id= "0";
            DeleteOnDeath = false;
            ProfileImage = "none";
        }
    }
}
