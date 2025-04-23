using MediatR;
using task06_Currencies.Currencies.UpdateCurrency;

namespace task06_Currencies.Services
{
    public class OutboxProcessorService(IServiceScopeFactory scopeFactory) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var timer = new PeriodicTimer(TimeSpan.FromSeconds(10));
            using var scope = scopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            while (!stoppingToken.IsCancellationRequested
                && await timer.WaitForNextTickAsync(stoppingToken))
            {
                await mediator.Send(new SynchronizeUpdatedCurrenciesCommand(), stoppingToken);
            }
        }
    }
}
