using Microsoft.EntityFrameworkCore;
using nbpTracker.Model.Entities;

namespace nbpTracker
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
        }

        public DbSet<ExchangeTable> ExchangeTables { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<CurrencyRate> CurrencyRates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CurrencyRate>()
                .HasIndex(c => new { c.ExchangeTableId, c.CurrencyId })
                .IsUnique();

            modelBuilder.Entity<Currency>()
                .HasIndex(c => c.CurrencyCode)
                .IsUnique();

            modelBuilder.Entity<ExchangeTable>()
                .HasIndex(t => t.No)
                .IsUnique();
        }
    }
}