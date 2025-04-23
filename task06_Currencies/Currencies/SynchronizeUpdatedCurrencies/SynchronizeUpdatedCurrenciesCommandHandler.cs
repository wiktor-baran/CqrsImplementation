using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System.Text.Json;
using task06_Currencies.Currencies.UpdateCurrencies;
using task06_Currencies.Repositories.ReadDb.Entities;
using task06_Currencies.Repositories.WriteDb;

namespace task06_Currencies.Currencies.UpdateCurrency
{
    public class SynchronizeUpdatedCurrenciesCommandHandler(CurrenciesDbContext sourceDbContext, IMongoDatabase mongoDatabase) : IRequestHandler<SynchronizeUpdatedCurrenciesCommand, bool>
    {
        private readonly IMongoCollection<CurrencyReadModel> collection = mongoDatabase.GetCollection<CurrencyReadModel>("CurrenciesDb");

        public async Task<bool> Handle(SynchronizeUpdatedCurrenciesCommand request, CancellationToken cancellationToken)
        {
            var outboxMessages = await sourceDbContext.OutboxMessages
                .Where(message => message.Type == typeof(CurrencyUpdated).FullName)
                .Where(message => message.ProcessedOn == null)
                .OrderBy(message => message.OccurredOn)
                .ToListAsync(cancellationToken);

            try
            {
                foreach (var outboxMessage in outboxMessages)
                {
                    var currency = JsonSerializer.Deserialize<CurrencyUpdated>(outboxMessage.Content);

                    if (currency == null)
                        continue;

                    var current = await collection
                        .Find(x => x.Symbol == currency.Symbol)
                        .FirstOrDefaultAsync(cancellationToken);

                    var model = new CurrencyReadModel
                    {
                        Id = current?.Id,
                        Name = currency.Name,
                        Symbol = currency.Symbol,
                        Rates = currency.Rates,
                    };

                    await collection.ReplaceOneAsync(
                        filter: x => x.Symbol == model.Symbol,
                        replacement: model,
                        options: new ReplaceOptions { IsUpsert = true },
                        cancellationToken: cancellationToken);

                    outboxMessage.ProcessedOn = DateTime.UtcNow;
                }
            }
            finally
            {
                await sourceDbContext.SaveChangesAsync(cancellationToken);
            }

            // mongosh
            // use CurrenciesDb
            // db.CurrenciesDb.find().pretty()

            return true;
        }
    }
}
