using System.ComponentModel.DataAnnotations;

namespace paymentManager.DTOs
{
    public class PaymentCreateRequest
    {
        public int PaymentId { get; set; } = 0;

        [Required]
        public int SupplierId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Leaf weight must be greater than 0")]
        public decimal LeafWeight { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Rate must be greater than 0")]
        public decimal Rate { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Gross amount cannot be negative")]
        public decimal GrossAmount { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Advance deduction cannot be negative")]
        public decimal AdvanceDeduction { get; set; } = 0;

        [Range(0, double.MaxValue, ErrorMessage = "Debt deduction cannot be negative")]
        public decimal DebtDeduction { get; set; } = 0;

        [Range(0, double.MaxValue, ErrorMessage = "Incentive addition cannot be negative")]
        public decimal IncentiveAddition { get; set; } = 0;

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Net amount cannot be negative")]
        public decimal NetAmount { get; set; }

        [Required]
        [StringLength(20)]
        public string PaymentMethod { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }

        public string CreatedBy { get; set; } = "System";
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
