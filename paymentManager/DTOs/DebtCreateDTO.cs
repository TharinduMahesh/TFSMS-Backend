using System.ComponentModel.DataAnnotations;

namespace paymentManager.DTOs
{
    public class DebtCreateDTO
    {
        [Required]
        public int SupplierId { get; set; }

        [Required]
        [StringLength(50)]
        public string DebtType { get; set; }

        [Required]
        [StringLength(200)]
        public string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total amount must be greater than 0")]
        public decimal TotalAmount { get; set; }

        [Required]
        public DateTime IssueDate { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Deductions made must be 0 or greater")]
        public decimal DeductionsMade { get; set; } = 0;

        [Range(1, 100, ErrorMessage = "Deduction percentage must be between 1 and 100")]
        public decimal DeductionPercentage { get; set; } = 20;
    }
}
