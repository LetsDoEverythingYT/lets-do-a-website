using System.Xml.Linq;

namespace lets_do_a_website.Data.Entities
{
    public class MatchEntry
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int Status { get; set; }

        public string ProfileImage { get; set; } = "none";
        public  int MatchId { get; set; }
        public MatchEntry(string name, int status, string profileImage)
        {
            Name = name;
            Status = status;
            ProfileImage = profileImage;
        }
    }
}
