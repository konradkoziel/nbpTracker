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
            var defaultDelayDays = 1;
            var configured = int.TryParse(_configuration["Params:TimeSpanDelayInDays"], out int delayDays) && delayDays > 0
                ? delayDays
                : defaultDelayDays;

            var interval = TimeSpan.FromDays(configured);

            _logger.LogInformation("CurrencySyncWorker started. Interval = {Interval}", interval);

            await RunOnceAsync(stoppingToken);

            using var timer = new PeriodicTimer(interval);
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await RunOnceAsync(stoppingToken);
            }
        }

        private async Task RunOnceAsync(CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var fetcher = scope.ServiceProvider.GetRequiredService<ICurrencyRatesFetcher>();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            _logger.LogInformation("Uruchamiam synchronizację kursów.");

            var result = await SaveTableToDatabase(fetcher, context, cancellationToken);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Synchronizacja zakończona z błędem: {Error}", result.Error);
            }
            else
            {
                _logger.LogInformation("Synchronizacja zakończona pomyślnie.");
            }
        }

        private async Task<Result> SaveTableToDatabase(ICurrencyRatesFetcher fetcher, AppDbContext context, CancellationToken cancellationToken)
        {
            var result = await fetcher.GetTableAsync(cancellationToken);
            var nbpTable = result.Value;

            if (!result.IsSuccess || nbpTable?.Rates == null || nbpTable.Rates.Count == 0)
                return Result.Fail("Bad response or no data.");

            var exchangeTablesAny = await context.ExchangeTables.AnyAsync(t => t.No == nbpTable.No, cancellationToken);

            if (exchangeTablesAny)
                return Result.Fail("Same table exists in database.");

            await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var existingCurrencies = await context.Currencies.ToListAsync(cancellationToken);
                var existingByCode = existingCurrencies.ToDictionary(c => c.CurrencyCode, c => c, StringComparer.OrdinalIgnoreCase);

                var exchangeTable = new ExchangeTable
                {
                    TableName = nbpTable.Table,
                    No = nbpTable.No,
                    EffectiveDate = nbpTable.EffectiveDate.UtcDateTime,
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
                        await context.Currencies.AddAsync(currency, cancellationToken);
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

                await context.ExchangeTables.AddAsync(exchangeTable, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return Result.Ok();
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Operacja zapisu została anulowana.");
                await transaction.RollbackAsync();
                return Result.Fail("Operation cancelled.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Błąd podczas zapisu tabeli kursów do bazy.");
                return Result.Fail("Exception occured while saving data.");
            }
        }
    }
}