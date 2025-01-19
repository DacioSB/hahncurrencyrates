using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hahn.CurrencyRates.Application.DTOs;
using Hahn.CurrencyRates.Domain.Repositories;
using MediatR;

namespace Hahn.CurrencyRates.Application.Queries;

public class GetCurrencyRatesQueryHandler 
    : IRequestHandler<GetCurrencyRatesQuery, IEnumerable<CurrencyRateDto>>
{
    private readonly ICurrencyRateQueryRepository _repository;

    public GetCurrencyRatesQueryHandler(ICurrencyRateQueryRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CurrencyRateDto>> Handle(
        GetCurrencyRatesQuery request, 
        CancellationToken cancellationToken)
    {
        var rates = request.BaseCurrency != null
            ? await _repository.GetLatestRatesForBaseCurrencyAsync(request.BaseCurrency, cancellationToken)
            : await _repository.GetAllLatestRatesAsync(cancellationToken);

        var dtos = rates
            .Select(r => new CurrencyRateDto(
                r.BaseCurrency,
                r.TargetCurrency,
                r.Rate,
                r.Timestamp))
            .AsEnumerable();

        // Apply target currencies filter if specified
        if (request.TargetCurrencies?.Any() == true)
        {
            dtos = dtos.Where(r => 
                request.TargetCurrencies.Any(tc => 
                    tc.Equals(r.TargetCurrency, StringComparison.OrdinalIgnoreCase)));
        }

        // Apply sorting
        if (!string.IsNullOrWhiteSpace(request.SortBy))
        {
            dtos = request.SortBy.ToLowerInvariant() switch
            {
                "basecurrency" => request.SortDescending 
                    ? dtos.OrderByDescending(r => r.BaseCurrency)
                    : dtos.OrderBy(r => r.BaseCurrency),
                
                "targetcurrency" => request.SortDescending
                    ? dtos.OrderByDescending(r => r.TargetCurrency)
                    : dtos.OrderBy(r => r.TargetCurrency),
                
                "rate" => request.SortDescending
                    ? dtos.OrderByDescending(r => r.Rate)
                    : dtos.OrderBy(r => r.Rate),
                
                "lastupdated" => request.SortDescending
                    ? dtos.OrderByDescending(r => r.LastUpdated)
                    : dtos.OrderBy(r => r.LastUpdated),
                
                _ => dtos
            };
        }

        return dtos;
    }
}
