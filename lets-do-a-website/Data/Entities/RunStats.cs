namespace lets_do_a_website.Data.Entities
{
    public class RunStats
    {
        public int Id { get; set; }
        public string Streamer { get; set; } = "";
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int DeathCount { get; set; }
        public string Deaths { get; set; } = "";

        public RunStats() { }
        public RunStats(string streamer, DateTime start, DateTime end, int count, string deaths)
        {
            Streamer = streamer;
            StartTime = start;
            EndTime = end;
            DeathCount = count;
            Deaths = deaths;
        }
    }
}
