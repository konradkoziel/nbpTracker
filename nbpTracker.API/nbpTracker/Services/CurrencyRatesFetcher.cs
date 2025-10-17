using nbpTracker.Common;
using nbpTracker.Model.ApiResponses;
using Newtonsoft.Json;

namespace nbpTracker.Services
{
    public class CurrencyRatesFetcher : ICurrencyRatesFetcher
    {

        private readonly ILogger<CurrencyRatesFetcher> _logger;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        public CurrencyRatesFetcher(ILogger<CurrencyRatesFetcher> logger, IConfiguration configuration, HttpClient httpClient)
        {
            _logger = logger;
            _configuration = configuration;
            _httpClient = httpClient;
        }
        public async Task<Result<NbpTable>> GetTableAsync()
        {
            string? url = _configuration["ApiSettings:NbpUrl"];

            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException("URL cannot be empty.", nameof(url));
            }
            try
            {
                var response = await _httpClient.GetAsync(url);

                response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();
                var nbpTable = JsonConvert.DeserializeObject<List<NbpTable>>(json)?.FirstOrDefault();
                return new Result<NbpTable>(true, nbpTable, null);

            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Błąd HTTP: {ex.Message}");
                return new Result<NbpTable>(false, null, $"{ex}");
            }
        }
    }
}
