using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Hahn.CurrencyRates.Domain.Entities;

namespace Hahn.CurrencyRates.Domain.Repositories;

public interface ICurrencyRateQueryRepository
{
    Task<IEnumerable<CurrencyRate>> GetAllLatestRatesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<CurrencyRate>> GetLatestRatesForBaseCurrencyAsync(string baseCurrency, CancellationToken cancellationToken = default);
    Task<CurrencyRate?> GetRateAsync(string baseCurrency, string targetCurrency, CancellationToken cancellationToken = default);
    Task UpsertRateAsync(CurrencyRate rate, CancellationToken cancellationToken = default);
}
