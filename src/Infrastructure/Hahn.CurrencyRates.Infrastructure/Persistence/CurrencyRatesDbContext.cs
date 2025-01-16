using Hahn.CurrencyRates.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hahn.CurrencyRates.Infrastructure.Persistence;

public class CurrencyRatesDbContext : DbContext
{
    public DbSet<CurrencyRate> CurrencyRates { get; set; }

    public CurrencyRatesDbContext(DbContextOptions<CurrencyRatesDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CurrencyRate>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.BaseCurrency)
                .IsRequired()
                .HasMaxLength(3);

            entity.Property(e => e.TargetCurrency)
                .IsRequired()
                .HasMaxLength(3);

            entity.Property(e => e.Rate)
                .HasPrecision(18, 8);

            entity.Property(e => e.Timestamp)
                .IsRequired();

            // Create a unique index on BaseCurrency and TargetCurrency
            entity.HasIndex(e => new { e.BaseCurrency, e.TargetCurrency })
                .IsUnique();
        });
    }
}
