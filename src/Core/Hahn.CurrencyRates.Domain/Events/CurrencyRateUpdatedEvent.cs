using System;
using MediatR;

namespace Hahn.CurrencyRates.Domain.Events;

public class CurrencyRateUpdatedEvent : INotification
{
    public string BaseCurrency { get; }
    public string TargetCurrency { get; }
    public decimal Rate { get; }
    public DateTime Timestamp { get; }

    public CurrencyRateUpdatedEvent(string baseCurrency, string targetCurrency, decimal rate, DateTime timestamp)
    {
        BaseCurrency = baseCurrency;
        TargetCurrency = targetCurrency;
        Rate = rate;
        Timestamp = timestamp;
    }
}
