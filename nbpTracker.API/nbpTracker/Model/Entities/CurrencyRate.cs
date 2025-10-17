using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace nbpTracker.Model.Entities
{
    [Table("CurrencyRates")]
    public class CurrencyRate
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,6)")]
        public decimal Mid { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(ExchangeTable))]
        public int ExchangeTableId { get; set; }
        public ExchangeTable ExchangeTable { get; set; } = null!;

        [ForeignKey(nameof(Currency))]
        public int CurrencyId { get; set; }
        public Currency Currency { get; set; } = null!;
    }
}
