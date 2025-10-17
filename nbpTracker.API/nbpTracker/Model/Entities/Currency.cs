using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace nbpTracker.Model.Entities
{
    public class Currency
    {
        [Key]
        public int Id { get; set; }

        [Required, Column(TypeName = "char(3)")]
        public string CurrencyCode { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string CurrencyName { get; set; } = string.Empty;

    }
}
