using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace paymentManager.Models
{
    public class Debt
    {
        [Key]
        public int DebtId { get; set; }

        [Required]
        public int SupplierId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Balance amount must be 0 or greater")]
        public decimal BalanceAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Deductions made must be 0 or greater")]
        public decimal DeductionsMade { get; set; } = 0;

        [Required]
        [StringLength(200)]
        public string Description { get; set; }

        [Required]
        [StringLength(50)]
        public string DebtType { get; set; } = "Loan";

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total amount must be greater than 0")]
        public decimal TotalAmount { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        [Range(1, 100, ErrorMessage = "Deduction percentage must be between 1 and 100")]
        public decimal DeductionPercentage { get; set; } = 20;

        [StringLength(20)]
        public string Status { get; set; } = "Active";

        [Required]
        public DateTime IssueDate { get; set; } = DateTime.Now;

        [StringLength(50)]
        public string? CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation property
        [ForeignKey("SupplierId")]
        public virtual Supplier? Supplier { get; set; }
    }
}
