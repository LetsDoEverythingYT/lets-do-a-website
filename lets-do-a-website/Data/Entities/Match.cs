namespace lets_do_a_website.Data.Entities
{
    public class Match
    {
        public int Id { get; set; } 
        public string Streamer { get; set; }

        public  List<MatchEntry> Entries { get; set; } = new List<MatchEntry>();


        public Match(string streamer)
        {
            Streamer = streamer;

        }

        public void RemoveParticipant(string guest) 
        {
            Entries.RemoveAll(e => e.Name.Equals(guest));
        }
        public void AddParticipant(string guest, int accepted=1, string profileImage = "none")
        {
            if (Entries.Any(e => e.Name.Equals(guest)))
                return;
            
            Entries.Add(new MatchEntry(guest, accepted, profileImage));
        }
        public MatchEntry getParticipant(string guest)
        {
            return (Entries.Where(e => e.Name.Equals(guest)).FirstOrDefault());
        }

    }
}
