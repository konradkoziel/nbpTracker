using nbpTracker.Model.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace nbpTracker.Model.Dto
{
    public class CurrencyRateDto
    {
        public int Id { get; set; }
        public decimal Mid { get; set; }
        public string? CurrencyCode { get; set; }
        public string? CurrencyName { get; set; }
    }
}
