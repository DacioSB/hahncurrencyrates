using System.Threading;
using System.Threading.Tasks;
using Hahn.CurrencyRates.Domain.Events;
using Hahn.CurrencyRates.Domain.Entities;
using Hahn.CurrencyRates.Domain.Repositories;
using MediatR;

namespace Hahn.CurrencyRates.Application.EventHandlers;

public class CurrencyRateUpdatedEventHandler : INotificationHandler<CurrencyRateUpdatedEvent>
{
    private readonly ICurrencyRateQueryRepository _queryRepository;

    public CurrencyRateUpdatedEventHandler(ICurrencyRateQueryRepository queryRepository)
    {
        _queryRepository = queryRepository;
    }

    public async Task Handle(CurrencyRateUpdatedEvent notification, CancellationToken cancellationToken)
    {
        var rate = new CurrencyRate(
            notification.BaseCurrency,
            notification.TargetCurrency,
            notification.Rate,
            notification.Timestamp);

        await _queryRepository.UpsertRateAsync(rate, cancellationToken);
    }
}
