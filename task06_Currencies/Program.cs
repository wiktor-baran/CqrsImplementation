using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using task06_Currencies.Repositories.WriteDb;
using task06_Currencies.Repositories.WriteDb.Entities;
using task06_Currencies.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

builder.Services.AddDbContext<CurrenciesDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("WriteDb")));

builder.Services.AddSingleton(sp =>
{
    var client = new MongoClient(builder.Configuration.GetConnectionString("ReadDb"));
    return client.GetDatabase("CurrenciesDb");
});

builder.Services.AddHostedService<OutboxProcessorService>();

builder.Services.AddControllers();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CurrenciesDbContext>();
    await context.Database.MigrateAsync();

    if (!await context.Currencies.AnyAsync())
    {
        await context.Currencies.AddAsync(new Currency
        {
            Name = "Euro",
            Symbol = "EUR",
            PriceEur = 1,
            UpdatedAtUtc = DateTime.UtcNow,
        });
        await context.SaveChangesAsync();
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
