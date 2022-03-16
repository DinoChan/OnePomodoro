using Microsoft.EntityFrameworkCore;

namespace DataAccessLibrary
{
    public class PeriodContext : DbContext
    {
        public static string Filename { get; set; }

        public DbSet<Period> Periods { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Period>().HasIndex(p => p.From);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite($"Filename={Filename}");
        }
    }
}
