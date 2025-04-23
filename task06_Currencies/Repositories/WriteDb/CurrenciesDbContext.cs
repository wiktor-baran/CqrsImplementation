using Microsoft.EntityFrameworkCore;
using task06_Currencies.Repositories.WriteDb.Entities;

namespace task06_Currencies.Repositories.WriteDb
{
    public class CurrenciesDbContext(DbContextOptions<CurrenciesDbContext> options) : DbContext(options)
    {
        public DbSet<Currency> Currencies { get; init; }
        public DbSet<OutboxMessage> OutboxMessages { get; init; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Currency>()
               .ToTable("Currencies")
               .HasKey(er => er.Id);

            modelBuilder.Entity<OutboxMessage>()
               .ToTable("OutboxMessages")
               .HasKey(er => er.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}
