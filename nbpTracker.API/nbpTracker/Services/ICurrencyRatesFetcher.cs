using nbpTracker.Common;
using nbpTracker.Model.ApiResponses;

namespace nbpTracker.Services
{
    public interface ICurrencyRatesFetcher
    {
        public Task<Result<NbpTable>> GetTableAsync(CancellationToken cancellationToken);
    }
}
