using Hangfire;
using Hahn.CurrencyRates.Jobs.CurrencyRates;
using Microsoft.Extensions.DependencyInjection;

namespace Hahn.CurrencyRates.Jobs;

public static class DependencyInjection
{
    public static IServiceCollection AddJobs(this IServiceCollection services)
    {
        services.AddScoped<FetchCurrencyRatesJob>();

        return services;
    }

    public static void ConfigureJobs(IRecurringJobManager recurringJobManager)
    {
        // Schedule the currency rates fetch job to run every hour
        recurringJobManager.AddOrUpdate<FetchCurrencyRatesJob>(
            "fetch-currency-rates",
            job => job.ExecuteAsync(default),
            Cron.Hourly());
    }
}
