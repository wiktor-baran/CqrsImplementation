using MediatR;

namespace task06_Currencies.Currencies.UpdateCurrencies
{
    public record UpdateCurrencyCommand : IRequest<bool>
    {
        public required string Symbol { get; init; }

        public string Name { get; init; } = string.Empty;

        public decimal PriceEur { get; init; }
    }
}
