using lets_do_a_website.Data.Entities;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using MySql.EntityFrameworkCore;

namespace lets_do_a_website.Data
{
    public class WTDContext : DbContext
    {
        public DbSet<UserSettings> UserSettings { get; set; }   
        public DbSet<Tracker> Trackers { get; set; }
        public DbSet<Permissions> Permissions { get; set; }
        public DbSet<RunStats> RunStats { get; set; }

        public WTDContext(DbContextOptions<WTDContext> ctx) : base(ctx)
        {
            
        }
        
    }
}