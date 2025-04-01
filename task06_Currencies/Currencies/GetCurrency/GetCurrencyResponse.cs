namespace task06_Currencies.Currencies.GetCurrency
{
    public record GetCurrencyResponse
    {
        public required string Symbol { get; init; }

        public string Name { get; init; } = string.Empty;

        public Dictionary<string, decimal> Rates { get; init; } = [];
    }
}
