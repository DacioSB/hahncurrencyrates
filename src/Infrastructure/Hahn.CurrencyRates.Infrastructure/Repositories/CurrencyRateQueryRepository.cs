using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hahn.CurrencyRates.Domain.Entities;
using Hahn.CurrencyRates.Domain.Repositories;
using Hahn.CurrencyRates.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Hahn.CurrencyRates.Infrastructure.Repositories;

public class CurrencyRateQueryRepository : ICurrencyRateQueryRepository
{
    private readonly QueryDbContext _context;

    public CurrencyRateQueryRepository(QueryDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CurrencyRate>> GetAllLatestRatesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.CurrencyRates
            .OrderByDescending(r => r.Timestamp)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<CurrencyRate>> GetLatestRatesForBaseCurrencyAsync(
        string baseCurrency, 
        CancellationToken cancellationToken = default)
    {
        return await _context.CurrencyRates
            .Where(r => r.BaseCurrency == baseCurrency.ToUpperInvariant())
            .OrderByDescending(r => r.Timestamp)
            .ToListAsync(cancellationToken);
    }

    public async Task<CurrencyRate?> GetRateAsync(
        string baseCurrency,
        string targetCurrency,
        CancellationToken cancellationToken = default)
    {
        return await _context.CurrencyRates
            .FirstOrDefaultAsync(r =>
                r.BaseCurrency == baseCurrency.ToUpperInvariant() &&
                r.TargetCurrency == targetCurrency.ToUpperInvariant(),
                cancellationToken);
    }

    public async Task UpsertRateAsync(CurrencyRate rate, CancellationToken cancellationToken = default)
    {
        var existingRate = await GetRateAsync(
            rate.BaseCurrency,
            rate.TargetCurrency,
            cancellationToken);

        if (existingRate == null)
        {
            _context.CurrencyRates.Add(rate);
        }
        else
        {
            existingRate.Update(rate.Rate, rate.Timestamp);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
