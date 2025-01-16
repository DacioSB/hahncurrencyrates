using System;
using MediatR;

namespace Hahn.CurrencyRates.Application.Commands;

public record UpsertCurrencyRateCommand(
    string BaseCurrency,
    string TargetCurrency,
    decimal Rate,
    DateTime Timestamp) : IRequest;
