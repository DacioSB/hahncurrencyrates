using Hahn.CurrencyRates.Application.DTOs;
using Hahn.CurrencyRates.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hahn.CurrencyRates.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CurrencyRatesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CurrencyRatesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CurrencyRateDto>>> GetCurrencyRates(
        [FromQuery] string? baseCurrency = null,
        [FromQuery] string[]? targetCurrencies = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] bool sortDescending = false,
        CancellationToken cancellationToken = default)
    {
        var query = new GetCurrencyRatesQuery(
            baseCurrency,
            targetCurrencies,
            sortBy,
            sortDescending);

        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}
