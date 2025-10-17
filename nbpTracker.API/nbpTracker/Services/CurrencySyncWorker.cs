using Microsoft.EntityFrameworkCore;
using nbpTracker.Common;
using nbpTracker.Model.Entities;

namespace nbpTracker.Services
{
    public class CurrencySyncWorker : BackgroundService
    {
        private readonly ILogger<CurrencySyncWorker> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;

        public CurrencySyncWorker(IServiceScopeFactory scopeFactory, ILogger<CurrencySyncWorker> logger, IConfiguration configuration)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var delayDays = 1; // Zabezpieczenie na wypadek gdy parser zawiedzie
            int.TryParse(_configuration["Params:TimeSpanDelayInDays"], out delayDays);
            var timer = new PeriodicTimer(TimeSpan.FromDays(delayDays));

            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                using var scope = _scopeFactory.CreateScope();
                var fetcher = scope.ServiceProvider.GetRequiredService<ICurrencyRatesFetcher>();
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

	            _logger.LogInformation("Wykonuję cyklicznie co: {Delay}", TimeSpan.FromDays(delayDays));

                await SaveTableToDatabase(fetcher, context);
            }
        }

        private async Task<Result> SaveTableToDatabase(ICurrencyRatesFetcher fetcher, AppDbContext context)
        {
            var result = await fetcher.GetTableAsync();
            var nbpTable = result.Value;

            if (!result.IsSuccess || nbpTable?.Rates == null || nbpTable.Rates.Count == 0)
                return Result.Fail("Bad response or no data.");

            var exchangeTablesAny = await context.ExchangeTables.AnyAsync(t => t.No == nbpTable.No);

            if (exchangeTablesAny)
                return Result.Fail("Same table exists in database.");


            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var existingCurrencies = await context.Currencies.ToListAsync();
                var existingByCode = existingCurrencies.ToDictionary(c => c.CurrencyCode, c => c, StringComparer.OrdinalIgnoreCase);

                var exchangeTable = new ExchangeTable
                {
                    TableName = nbpTable.Table,
                    No = nbpTable.No,
                    EffectiveDate = nbpTable.EffectiveDate.DateTime,
                    CreatedAt = DateTime.UtcNow,
                    CurrencyRates = new List<CurrencyRate>()
                };

                foreach (var rate in nbpTable.Rates)
                {
                    if (!existingByCode.TryGetValue(rate.Code, out var currency))
                    {
                        currency = new Currency
                        {
                            CurrencyCode = rate.Code,
                            CurrencyName = rate.Currency
                        };
                        await context.Currencies.AddAsync(currency);
                        existingByCode[rate.Code] = currency;
                    }

                    exchangeTable.CurrencyRates.Add(new CurrencyRate
                    {
                        Mid = Convert.ToDecimal(rate.Mid),
                        CreatedAt = DateTime.UtcNow,
                        Currency = currency,
                        ExchangeTable = exchangeTable
                    });
                }

                await context.ExchangeTables.AddAsync(exchangeTable);
                await context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Result.Ok();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex.Message, ex);
                return Result.Fail($"Exception occured: {ex.Message}");
            }

        }
    }
}

