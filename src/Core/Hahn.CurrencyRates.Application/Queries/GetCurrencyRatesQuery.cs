using System.Collections.Generic;
using Hahn.CurrencyRates.Application.DTOs;
using MediatR;

namespace Hahn.CurrencyRates.Application.Queries;

public record GetCurrencyRatesQuery(
    string? BaseCurrency = null,
    string? TargetCurrency = null,
    string? SortBy = null,
    bool SortDescending = false) : IRequest<IEnumerable<CurrencyRateDto>>;
