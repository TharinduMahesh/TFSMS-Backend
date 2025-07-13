using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace paymentManager.Models
{
    public class Incentive
    {
        [Key]
        public int IncentiveId { get; set; }

        [Required]
        public int SupplierId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Quality bonus must be a positive value")]
        public decimal QualityBonus { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Loyalty bonus must be a positive value")]
        public decimal LoyaltyBonus { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        [StringLength(7)] // Format: YYYY-MM
        [RegularExpression(@"^\d{4}-\d{2}$", ErrorMessage = "Month must be in YYYY-MM format")]
        public string Month { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("SupplierId")]
        public virtual Supplier? Supplier { get; set; }
    }
}
