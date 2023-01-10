namespace lets_do_a_website.Data.Entities
{
    public class RunStats
    {
        public int Id { get; set; }
        public string Streamer { get; set; }
        public DateTime Submitted { get; set; }
        public string Deaths { get; set; }

        public RunStats() { }
        public RunStats(string streamer, DateTime submitted, string deaths)
        {
            Streamer = streamer;
            Submitted = submitted;
            Deaths = deaths;
        }
    }
}
