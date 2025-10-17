using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace nbpTracker.Model.Dto
{
    public class ExchangeTableDto
    {
        public int Id { get; set; }
        public string TableName { get; set; }
        public string No { get; set; }
        public DateTime EffectiveDate { get; set; }
        public ICollection<CurrencyRateDto> CurrencyRates { get; set; } = new List<CurrencyRateDto>();
    }
}
