using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using nbpTracker.Services;

namespace nbpTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyRatesController : ControllerBase
    {
        private readonly ICurrencyRatesService _currencyRatesService;
        public CurrencyRatesController(ICurrencyRatesService currencyRatesService)
        {
            _currencyRatesService = currencyRatesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTable()
        {
            var result = await _currencyRatesService.GetTableAsync();

            if (result.IsSuccess)
            {
                if(result.Value == null) return NotFound("No table found.");
                return Ok(result.Value);
            }
            return BadRequest(result.Error);

        }
    }
}
