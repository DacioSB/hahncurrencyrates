using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hahn.CurrencyRates.Application.Commands;
using Hahn.CurrencyRates.Domain.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hahn.CurrencyRates.Jobs.CurrencyRates;

public class FetchCurrencyRatesJob
{
    private readonly IExternalRatesService _externalRatesService;
    private readonly IMediator _mediator;
    private readonly ILogger<FetchCurrencyRatesJob> _logger;

    private static readonly string[] AvailableCurrencies = { "USD", "EUR", "GBP", "JPY" };

    public FetchCurrencyRatesJob(
        IExternalRatesService externalRatesService,
        IMediator mediator,
        ILogger<FetchCurrencyRatesJob> logger)
    {
        _externalRatesService = externalRatesService;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task ExecuteAsync(string baseCurrency, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Starting to fetch currency rates for base currency {BaseCurrency} at {Time}", 
                baseCurrency, DateTime.UtcNow);

            // Get target currencies (all currencies except the base currency)
            var targetCurrencies = AvailableCurrencies
                .Where(c => c != baseCurrency)
                .ToArray();

            var response = await _externalRatesService.GetLatestRatesAsync(
                baseCurrency,
                targetCurrencies,
                cancellationToken);

            foreach (var (currency, rate) in response.Rates)
            {
                await _mediator.Send(new UpsertCurrencyRateCommand(
                    baseCurrency,
                    currency,
                    rate,
                    response.Timestamp), cancellationToken);

                _logger.LogInformation(
                    "Updated rate for {BaseCurrency}/{TargetCurrency}: {Rate}",
                    baseCurrency,
                    currency,
                    rate);
            }

            _logger.LogInformation("Completed fetching currency rates for base currency {BaseCurrency} at {Time}", 
                baseCurrency, DateTime.UtcNow);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching currency rates for base currency {BaseCurrency}", 
                baseCurrency);
            throw;
        }
    }

    public async Task ExecuteForAllBaseCurrenciesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var baseCurrency in AvailableCurrencies)
        {
            await ExecuteAsync(baseCurrency, cancellationToken);
        }
    }
}
