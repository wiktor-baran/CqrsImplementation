using MediatR;
using Microsoft.EntityFrameworkCore;
using task06_Currencies.Repositories;
using task06_Currencies.Repositories.Entities;

namespace task06_Currencies.Currencies.UpdateCurrencies
{
    public class UpdateCurrencyCommandHandler(CurrenciesDbContext dbContext) : IRequestHandler<UpdateCurrencyCommand, bool>
    {
        public async Task<bool> Handle(UpdateCurrencyCommand request, CancellationToken cancellationToken)
        {
            var currency = await dbContext.Currencies.FirstOrDefaultAsync(
                c => c.Symbol == request.Symbol,
                cancellationToken);

            if (currency == null)
            {
                await dbContext.Currencies.AddAsync(new Currency
                {
                    Symbol = request.Symbol,
                    Name = request.Name,
                    PriceEur = request.PriceEur,
                    UpdatedAtUtc = DateTime.UtcNow,
                }, cancellationToken);

                await dbContext.SaveChangesAsync(cancellationToken);

                return true;
            }

            currency.PriceEur = request.PriceEur;
            currency.UpdatedAtUtc = DateTime.UtcNow;

            await dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
