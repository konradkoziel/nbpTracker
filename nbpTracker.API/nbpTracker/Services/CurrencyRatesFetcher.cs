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

        public async Task<Result<NbpTable>> GetTableAsync(CancellationToken cancellationToken = default)
        {
            string? url = _configuration["ApiSettings:NbpUrl"];
            if (string.IsNullOrEmpty(url))
            {
                _logger.LogError("ApiSettings:NbpUrl not configured.");
                return Result<NbpTable>.Fail("API URL not configured.");
            }

            try
            {
                using var response = await _httpClient.GetAsync(url, cancellationToken);
                var json = await response.Content.ReadAsStringAsync(cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Nieudane żądanie do NBP: {StatusCode}", response.StatusCode);
                    return Result<NbpTable>.Fail($"Failed to fetch data: {(int)response.StatusCode}");
                }

                var nbpTable = JsonConvert.DeserializeObject<List<NbpTable>>(json)?.FirstOrDefault();

                if (nbpTable == null)
                {
                    _logger.LogWarning("Pobrano poprawny status, ale brak danych JSON w odpowiedzi.");
                    return Result<NbpTable>.Fail("No data in response.");
                }

                return Result<NbpTable>.Ok(nbpTable);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Pobieranie kursów zostało anulowane.");
                return Result<NbpTable>.Fail("Operation cancelled.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas pobierania kursów z NBP.");
                return Result<NbpTable>.Fail("Error while fetching data.");
            }
        }
    }
}