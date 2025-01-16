using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hahn.CurrencyRates.Domain.Entities;
using Hahn.CurrencyRates.Domain.Repositories;
using Hahn.CurrencyRates.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Hahn.CurrencyRates.Infrastructure.Repositories;

public class CurrencyRateRepository : ICurrencyRateRepository
{
    private readonly CurrencyRatesDbContext _context;

    public CurrencyRateRepository(CurrencyRatesDbContext context)
    {
        _context = context;
    }

    public async Task<CurrencyRate?> GetByBaseCurrencyAndTargetAsync(
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

    public async Task<IEnumerable<CurrencyRate>> GetAllLatestRatesAsync(
        CancellationToken cancellationToken = default)
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

    public async Task<CurrencyRate> AddAsync(
        CurrencyRate rate,
        CancellationToken cancellationToken = default)
    {
        var entry = await _context.CurrencyRates.AddAsync(rate, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entry.Entity;
    }

    public async Task UpdateAsync(
        CurrencyRate rate,
        CancellationToken cancellationToken = default)
    {
        _context.CurrencyRates.Update(rate);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(
        string baseCurrency,
        string targetCurrency,
        CancellationToken cancellationToken = default)
    {
        return await _context.CurrencyRates
            .AnyAsync(r =>
                r.BaseCurrency == baseCurrency.ToUpperInvariant() &&
                r.TargetCurrency == targetCurrency.ToUpperInvariant(),
                cancellationToken);
    }
}
