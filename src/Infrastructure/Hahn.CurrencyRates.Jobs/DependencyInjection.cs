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
        // Schedule the currency rates fetch job to run every hour for all base currencies
        recurringJobManager.AddOrUpdate<FetchCurrencyRatesJob>(
            "fetch-all-currency-rates",
            job => job.ExecuteForAllBaseCurrenciesAsync(default),
            //each 5 minutes
            Cron.Hourly
            );
    }
}
