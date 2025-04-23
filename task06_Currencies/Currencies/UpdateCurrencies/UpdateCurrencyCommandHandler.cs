using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using task06_Currencies.Repositories.WriteDb;
using task06_Currencies.Repositories.WriteDb.Entities;

namespace task06_Currencies.Currencies.UpdateCurrencies
{
    public class UpdateCurrencyCommandHandler(CurrenciesDbContext dbContext) : IRequestHandler<UpdateCurrencyCommand, bool>
    {
        public async Task<bool> Handle(UpdateCurrencyCommand request, CancellationToken cancellationToken)
        {
            var currencyToUpdate = await dbContext.Currencies.FirstOrDefaultAsync(
                c => c.Symbol == request.Symbol,
                cancellationToken);

            if (currencyToUpdate != null)
            {
                currencyToUpdate.PriceEur = request.PriceEur;
                currencyToUpdate.UpdatedAtUtc = DateTime.UtcNow;
            }
            else
            {
                currencyToUpdate = new Currency
                {
                    Symbol = request.Symbol,
                    Name = request.Name,
                    PriceEur = request.PriceEur,
                    UpdatedAtUtc = DateTime.UtcNow,
                };

                await dbContext.Currencies.AddAsync(currencyToUpdate, cancellationToken);
            }
            await dbContext.SaveChangesAsync(cancellationToken);

            var allCurrencies = await dbContext.Currencies.ToListAsync(cancellationToken);
            foreach (var currency in allCurrencies)
            {
                var currencyUpdated = new CurrencyUpdated
                {
                    Name = currency.Name,
                    Symbol = currency.Symbol,
                    Rates = allCurrencies
                        .Where(c => c.Symbol != currency.Symbol)
                        .Select(c => new KeyValuePair<string, decimal>(c.Symbol, c.PriceEur / currency.PriceEur))
                        .ToDictionary(),
                    UpdatedAtUtc = currency.UpdatedAtUtc,
                };

                var outboxMessage = new OutboxMessage
                {
                    Id = Guid.NewGuid(),
                    OccurredOn = DateTime.UtcNow,
                    Type = typeof(CurrencyUpdated).FullName!,
                    Content = JsonSerializer.Serialize(currencyUpdated)
                };
                await dbContext.OutboxMessages.AddAsync(outboxMessage, cancellationToken);
            }

            await dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
