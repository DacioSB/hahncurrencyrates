using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Hahn.CurrencyRates.Domain.Services;

public interface IExternalRatesService
{
    Task<ExternalRatesResponse> GetLatestRatesAsync(
        string baseCurrency,
        IEnumerable<string> targetCurrencies,
        CancellationToken cancellationToken = default);
}

public record ExternalRatesResponse(
    bool Success,
    string Base,
    DateTime Timestamp,
    IDictionary<string, decimal> Rates);
