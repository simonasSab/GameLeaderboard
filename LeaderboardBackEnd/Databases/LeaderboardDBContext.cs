using LeaderboardBackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace LeaderboardBackEnd.Databases
{
    public class LeaderboardDBContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Score> Scores { get; set; }
        public DbSet<Level> Levels { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-OD4Q280;Database=GameLeaderboard;Integrated Security=True;TrustServerCertificate=true;");
        }
    }
}
