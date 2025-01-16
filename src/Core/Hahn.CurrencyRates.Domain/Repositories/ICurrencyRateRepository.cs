using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Hahn.CurrencyRates.Domain.Entities;

namespace Hahn.CurrencyRates.Domain.Repositories;

public interface ICurrencyRateRepository
{
    Task<CurrencyRate?> GetByBaseCurrencyAndTargetAsync(
        string baseCurrency, 
        string targetCurrency, 
        CancellationToken cancellationToken = default);

    Task<IEnumerable<CurrencyRate>> GetAllLatestRatesAsync(
        CancellationToken cancellationToken = default);

    Task<IEnumerable<CurrencyRate>> GetLatestRatesForBaseCurrencyAsync(
        string baseCurrency, 
        CancellationToken cancellationToken = default);

    Task<CurrencyRate> AddAsync(
        CurrencyRate rate, 
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        CurrencyRate rate, 
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(
        string baseCurrency, 
        string targetCurrency, 
        CancellationToken cancellationToken = default);
}
