using MediatR;

namespace task06_Currencies.Currencies.UpdateCurrency
{
    public record SynchronizeUpdatedCurrenciesCommand : IRequest<bool>;
}
