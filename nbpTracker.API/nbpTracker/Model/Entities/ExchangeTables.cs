using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace nbpTracker.Model.Entities
{
    public class ExchangeTable
    {
        [Key]
        public int Id { get; set; }

        [Required, Column(TypeName = "char(1)")]
        public string TableName { get; set; } = "B";

        [Required, MaxLength(20)]
        public string No { get; set; } = string.Empty;

        [Required]
        public DateTime EffectiveDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<CurrencyRate> CurrencyRates { get; set; } = new List<CurrencyRate>();
    }
}
