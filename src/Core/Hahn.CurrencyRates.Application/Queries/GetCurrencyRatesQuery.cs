using System.Collections.Generic;
using Hahn.CurrencyRates.Application.DTOs;
using MediatR;

namespace Hahn.CurrencyRates.Application.Queries;

public record GetCurrencyRatesQuery(
    string? BaseCurrency = null,
    IEnumerable<string>? TargetCurrencies = null,
    string? SortBy = null,
    bool SortDescending = false) : IRequest<IEnumerable<CurrencyRateDto>>;
