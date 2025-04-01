using MediatR;
using MongoDB.Driver;
using task06_Currencies.Repositories.Entities;

namespace task06_Currencies.Currencies.GetCurrency
{
    public class GetCurrencyQueryHandler(IMongoDatabase mongoDatabase) : IRequestHandler<GetCurrencyQuery, GetCurrencyResponse?>
    {
        private readonly IMongoCollection<CurrencyReadModel> collection = mongoDatabase.GetCollection<CurrencyReadModel>("CurrenciesDb");

        public async Task<GetCurrencyResponse?> Handle(GetCurrencyQuery query, CancellationToken cancellationToken)
        {
            var currency = await collection
                .Find(c => c.Symbol == query.Symbol)
                .SingleOrDefaultAsync(cancellationToken);

            return currency == null
                ? null
                : new GetCurrencyResponse
                {
                    Name = currency.Name,
                    Symbol = currency.Symbol,
                    Rates = currency.Rates,
                };
        }
    }
}
