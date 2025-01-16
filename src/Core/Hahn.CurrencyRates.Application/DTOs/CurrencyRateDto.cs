using System;

namespace Hahn.CurrencyRates.Application.DTOs;

public record CurrencyRateDto(
    string BaseCurrency,
    string TargetCurrency,
    decimal Rate,
    DateTime LastUpdated);
