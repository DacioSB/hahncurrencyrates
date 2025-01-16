using System;

namespace Hahn.CurrencyRates.Domain.Entities;

public class CurrencyRate
{
    public long Id { get; private set; }
    public string BaseCurrency { get; private set; }
    public string TargetCurrency { get; private set; }
    public decimal Rate { get; private set; }
    public DateTime Timestamp { get; private set; }

    private CurrencyRate() { } // For EF Core

    public CurrencyRate(string baseCurrency, string targetCurrency, decimal rate, DateTime timestamp)
    {
        if (string.IsNullOrWhiteSpace(baseCurrency))
            throw new ArgumentException("Base currency cannot be empty", nameof(baseCurrency));
        
        if (string.IsNullOrWhiteSpace(targetCurrency))
            throw new ArgumentException("Target currency cannot be empty", nameof(targetCurrency));
        
        if (rate <= 0)
            throw new ArgumentException("Rate must be greater than zero", nameof(rate));

        BaseCurrency = baseCurrency.ToUpperInvariant();
        TargetCurrency = targetCurrency.ToUpperInvariant();
        Rate = rate;
        Timestamp = timestamp;
    }

    public void Update(decimal newRate, DateTime newTimestamp)
    {
        if (newRate <= 0)
            throw new ArgumentException("Rate must be greater than zero", nameof(newRate));

        Rate = newRate;
        Timestamp = newTimestamp;
    }
}
