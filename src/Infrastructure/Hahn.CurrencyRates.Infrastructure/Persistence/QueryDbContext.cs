using Hahn.CurrencyRates.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Hahn.CurrencyRates.Infrastructure.Persistence;

public class QueryDbContext : DbContext
{
    public QueryDbContext(DbContextOptions<QueryDbContext> options) : base(options)
    {
    }

    public DbSet<CurrencyRate> CurrencyRates { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Use the same configuration as CurrencyRatesDbContext
        modelBuilder.Entity<CurrencyRate>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.BaseCurrency).IsRequired().HasMaxLength(3);
            entity.Property(e => e.TargetCurrency).IsRequired().HasMaxLength(3);
            entity.Property(e => e.Rate).HasPrecision(18, 8);
            entity.Property(e => e.Timestamp);

            // Add unique index on BaseCurrency + TargetCurrency
            entity.HasIndex(e => new { e.BaseCurrency, e.TargetCurrency })
                .IsUnique()
                .HasDatabaseName("IX_CurrencyRates_BaseCurrency_TargetCurrency");
        });
    }
}

/// <summary>
/// Factory for creating QueryDbContext instances at design time (for migrations).
/// Requires the QUERY_CONNECTION_STRING environment variable to be set.
/// Example: "Server=localhost;Database=HahnCurrencyRatesQuery;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True"
/// </summary>
public class QueryDbContextFactory : IDesignTimeDbContextFactory<QueryDbContext>
{
    public QueryDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("QUERY_CONNECTION_STRING");
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException(
                "QUERY_CONNECTION_STRING environment variable is not set. " +
                "This is required for database migrations.");
        }

        var optionsBuilder = new DbContextOptionsBuilder<QueryDbContext>();
        optionsBuilder.UseSqlServer(
            connectionString,
            b => b.MigrationsAssembly(typeof(QueryDbContext).Assembly.FullName));

        return new QueryDbContext(optionsBuilder.Options);
    }
}
