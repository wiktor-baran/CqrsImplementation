using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace task06_Currencies.Repositories.Entities
{
    public class Currency
    {
        [Key]
        public Guid Id { get; }

        [Required]
        public string Name { get; init; } = string.Empty;

        [Required]
        [StringLength(3)]
        public string Symbol { get; init; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PriceEur { get; set; }

        [Required]
        public DateTime UpdatedAtUtc { get; set; }
    }

}
