using AutoMapper;
using Microsoft.EntityFrameworkCore;
using nbpTracker.Common;
using nbpTracker.Model.Dto;
using nbpTracker.Model.Entities;

namespace nbpTracker.Services
{
    public class CurrencyRatesService : ICurrencyRatesService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public CurrencyRatesService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<ExchangeTableDto>> GetTableAsync()
        {
            var table = await _context.ExchangeTables.Include(t => t.CurrencyRates)
                .ThenInclude(cr => cr.Currency)
                .OrderByDescending(t => t.EffectiveDate)
                .FirstOrDefaultAsync();

            var tableDto = _mapper.Map<ExchangeTableDto>(table);


            if (table == null) { 
                return Result<ExchangeTableDto>.Fail("No exchange table found.");
            }
            return Result<ExchangeTableDto>.Ok(tableDto); 
        }
        
    }
}