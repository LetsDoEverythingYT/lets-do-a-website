namespace lets_do_a_website.Data.Entities
{
    public class Invite
    {
        public int Id { get; set; }
        public int MatchId { get; set; }  
        public string HostStreamer { get; set; }
        public string GuestStreamer { get; set; }

        public Invite(int matchId, string hostStreamer, string guestStreamer)
        {
            MatchId = matchId;
            HostStreamer = hostStreamer;
            GuestStreamer = guestStreamer;
        }
    }
}
