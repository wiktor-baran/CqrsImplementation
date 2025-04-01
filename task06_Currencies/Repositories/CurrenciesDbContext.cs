using Microsoft.EntityFrameworkCore;
using task06_Currencies.Repositories.Entities;

namespace task06_Currencies.Repositories
{
    public class CurrenciesDbContext : DbContext
    {
        public CurrenciesDbContext(DbContextOptions<CurrenciesDbContext> options)
            : base(options)
        {
            
        }

        public DbSet<Currency> Currencies { get; init; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Currency>()
                .ToTable("Currencies")
                .HasKey(er => er.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}
