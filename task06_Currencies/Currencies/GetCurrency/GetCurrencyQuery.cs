using MediatR;

namespace task06_Currencies.Currencies.GetCurrency
{
    public record GetCurrencyQuery : IRequest<GetCurrencyResponse>
    {
        public string Symbol { get; init; } = string.Empty;
    }
}
