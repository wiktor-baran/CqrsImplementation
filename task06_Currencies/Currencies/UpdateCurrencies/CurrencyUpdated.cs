namespace task06_Currencies.Currencies.UpdateCurrencies
{
    public class CurrencyUpdated
    {
        public required string Name { get; init; }

        public required string Symbol { get; init; }

        public required Dictionary<string, decimal> Rates { get; set; }

        public required DateTime UpdatedAtUtc { get; set; }
    }
}
