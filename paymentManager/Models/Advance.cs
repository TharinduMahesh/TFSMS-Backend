using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace paymentManager.Models
{
    public class Advance
    {
        [Key]
        public int AdvanceId { get; set; }

        [Required]
        public int SupplierId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Advance amount must be greater than 0")]
        public decimal AdvanceAmount { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Balance amount must be 0 or greater")]
        public decimal BalanceAmount { get; set; }

        [Required]
        [StringLength(200)]
        public string Purpose { get; set; }

        [Required]
        [StringLength(50)]
        public string AdvanceType { get; set; } = "Cash";

        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Recovered amount must be 0 or greater")]
        public decimal RecoveredAmount { get; set; } = 0;

        [Column(TypeName = "decimal(5,2)")]
        [Range(0, 100, ErrorMessage = "Recovery percentage must be between 0 and 100")]
        public decimal RecoveryPercentage { get; set; } = 30;

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
