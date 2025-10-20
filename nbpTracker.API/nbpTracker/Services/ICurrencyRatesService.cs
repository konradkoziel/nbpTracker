using nbpTracker.Common;
using nbpTracker.Model.Dto;

namespace nbpTracker.Services
{
    public interface ICurrencyRatesService
    {
        public Task<Result<ExchangeTableDto>> GetTableAsync(CancellationToken token);
    }
}
