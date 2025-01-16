using System;
using System.Collections.Generic;
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

    private static readonly string[] TargetCurrencies = { "EUR", "GBP", "JPY" };
    private const string BaseCurrency = "USD";

    public FetchCurrencyRatesJob(
        IExternalRatesService externalRatesService,
        IMediator mediator,
        ILogger<FetchCurrencyRatesJob> logger)
    {
        _externalRatesService = externalRatesService;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Starting to fetch currency rates at {Time}", DateTime.UtcNow);

            var response = await _externalRatesService.GetLatestRatesAsync(
                BaseCurrency,
                TargetCurrencies,
                cancellationToken);

            foreach (var (currency, rate) in response.Rates)
            {
                await _mediator.Send(new UpsertCurrencyRateCommand(
                    BaseCurrency,
                    currency,
                    rate,
                    response.Timestamp), cancellationToken);

                _logger.LogInformation(
                    "Updated rate for {BaseCurrency}/{TargetCurrency}: {Rate}",
                    BaseCurrency,
                    currency,
                    rate);
            }

            _logger.LogInformation("Completed fetching currency rates at {Time}", DateTime.UtcNow);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching currency rates");
            throw;
        }
    }
}
