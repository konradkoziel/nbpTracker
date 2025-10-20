using AutoMapper;
using Microsoft.EntityFrameworkCore;
using nbpTracker.Common;
using nbpTracker.Model.ApiResponses;
using nbpTracker.Model.Dto;

namespace nbpTracker.Services
{
    public class CurrencyRatesService : ICurrencyRatesService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ICurrencyRatesService> _logger;
        public CurrencyRatesService(AppDbContext context, IMapper mapper, ILogger<ICurrencyRatesService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<ExchangeTableDto>> GetTableAsync(CancellationToken token)
        {
            try
            {
                var table = await _context.ExchangeTables.Include(t => t.CurrencyRates)
                .ThenInclude(cr => cr.Currency)
                .OrderByDescending(t => t.EffectiveDate)
                .FirstOrDefaultAsync(token);

                if (table == null)
                {
                    return Result<ExchangeTableDto>.Fail("No exchange table found.");
                }

                var tableDto = _mapper.Map<ExchangeTableDto>(table);

                return Result<ExchangeTableDto>.Ok(tableDto);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Pobieranie kursów zostało anulowane.");
                return Result<ExchangeTableDto>.Fail("Operation cancelled.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas pobierania kursów z NBP.");
                return Result<ExchangeTableDto>.Fail("Error while fetching data.");
            }
        }
        
    }
}