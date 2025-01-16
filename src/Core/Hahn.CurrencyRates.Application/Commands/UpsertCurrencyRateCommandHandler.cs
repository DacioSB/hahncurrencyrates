using System.Threading;
using System.Threading.Tasks;
using Hahn.CurrencyRates.Domain.Entities;
using Hahn.CurrencyRates.Domain.Events;
using Hahn.CurrencyRates.Domain.Repositories;
using MediatR;

namespace Hahn.CurrencyRates.Application.Commands;

public class UpsertCurrencyRateCommandHandler : IRequestHandler<UpsertCurrencyRateCommand>
{
    private readonly ICurrencyRateRepository _repository;
    private readonly IPublisher _publisher;

    public UpsertCurrencyRateCommandHandler(ICurrencyRateRepository repository, IPublisher publisher)
    {
        _repository = repository;
        _publisher = publisher;
    }

    public async Task Handle(UpsertCurrencyRateCommand request, CancellationToken cancellationToken)
    {
        var existingRate = await _repository.GetByBaseCurrencyAndTargetAsync(
            request.BaseCurrency,
            request.TargetCurrency,
            cancellationToken);

        if (existingRate == null)
        {
            var newRate = new CurrencyRate(
                request.BaseCurrency,
                request.TargetCurrency,
                request.Rate,
                request.Timestamp);

            await _repository.AddAsync(newRate, cancellationToken);
        }
        else
        {
            existingRate.Update(request.Rate, request.Timestamp);
            await _repository.UpdateAsync(existingRate, cancellationToken);
        }

        // Publish event for the read side to update
        await _publisher.Publish(
            new CurrencyRateUpdatedEvent(
                request.BaseCurrency,
                request.TargetCurrency,
                request.Rate,
                request.Timestamp),
            cancellationToken);
    }
}
