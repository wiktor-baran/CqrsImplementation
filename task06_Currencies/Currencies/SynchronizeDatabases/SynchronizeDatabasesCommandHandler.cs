using MediatR;
using MongoDB.Driver;
using task06_Currencies.Repositories;
using task06_Currencies.Repositories.Entities;

namespace task06_Currencies.Currencies.SynchronizeDatabases
{
    public class SynchronizeDatabasesCommandHandler(CurrenciesDbContext sourceDbContext, IMongoDatabase mongoDatabase) : IRequestHandler<SynchronizeDatabasesCommand, bool>
    {
        private readonly IMongoCollection<CurrencyReadModel> collection = mongoDatabase.GetCollection<CurrencyReadModel>("CurrenciesDb");

        public async Task<bool> Handle(SynchronizeDatabasesCommand request, CancellationToken cancellationToken)
        {
            var currencies = sourceDbContext.Currencies
                .ToList();

            foreach (var currency in currencies)
            {
                var current = await collection
                    .Find(x => x.Symbol == currency.Symbol)
                    .FirstOrDefaultAsync(cancellationToken);

                var model = new CurrencyReadModel
                {
                    Id = current?.Id,
                    Name = currency.Name,
                    Symbol = currency.Symbol,
                    Rates = currencies
                        .Where(c => c.Symbol != currency.Symbol)
                        .Select(c => new KeyValuePair<string, decimal>(c.Symbol, c.PriceEur / currency.PriceEur))
                        .ToDictionary()
                };

                await collection.ReplaceOneAsync(
                    filter: x => x.Symbol == model.Symbol,
                    replacement: model,
                    options: new ReplaceOptions { IsUpsert = true },
                    cancellationToken: cancellationToken);
            }

            // mongosh
            // use CurrenciesDb
            // db.CurrenciesDb.find().pretty()

            return true;
        }
    }
}
