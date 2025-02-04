using System;
using Hahn.CurrencyRates.Domain.Repositories;
using Hahn.CurrencyRates.Domain.Services;
using Hahn.CurrencyRates.Infrastructure.Configuration;
using Hahn.CurrencyRates.Infrastructure.Persistence;
using Hahn.CurrencyRates.Infrastructure.Repositories;
using Hahn.CurrencyRates.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hahn.CurrencyRates.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        bool includeQueryContext = false)
    {
        // Register Command DbContext
        services.AddDbContext<CurrencyRatesDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(CurrencyRatesDbContext).Assembly.FullName)));

        // Register Query DbContext if needed (for API)
        if (includeQueryContext)
        {
            services.AddDbContext<QueryDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("QueryConnection"),
                    b => b.MigrationsAssembly(typeof(CurrencyRatesDbContext).Assembly.FullName)));
        }

        // Register Repositories
        services.AddScoped<ICurrencyRateRepository, CurrencyRateRepository>();
        
        if (includeQueryContext)
        {
            services.AddScoped<ICurrencyRateQueryRepository, CurrencyRateQueryRepository>();
        }

        // Register External Rates Service
        services.Configure<ExternalRatesApiOptions>(
            configuration.GetSection(ExternalRatesApiOptions.SectionName));

        services.AddHttpClient<IExternalRatesService, ExternalRatesService>()
            .ConfigureHttpClient(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
            });

        return services;
    }
}
