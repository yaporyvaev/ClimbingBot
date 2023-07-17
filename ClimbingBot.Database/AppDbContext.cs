using ClimbingBot.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClimbingBot.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<PollHistory> PollHistories { get; set; }
        public DbSet<TelegramGroup> TelegramGroups { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}