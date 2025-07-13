//using System.ComponentModel.DataAnnotations;

//namespace paymentManager.DTOs
//{
//    public class PaymentCalculationRequest
//    {
//        [Required]
//        public int SupplierId { get; set; }

//        [Required]
//        [Range(0.01, double.MaxValue, ErrorMessage = "Leaf weight must be greater than 0")]
//        public decimal LeafWeight { get; set; }

//        [Required]
//        [Range(0.01, double.MaxValue, ErrorMessage = "Rate must be greater than 0")]
//        public decimal Rate { get; set; }

//        public bool IncludeAdvances { get; set; } = true;
//        public bool IncludeDebts { get; set; } = true;
//        public bool IncludeIncentives { get; set; } = true;

//        public decimal? AdvanceDeductionLimit { get; set; }
//        public decimal? DebtDeductionLimit { get; set; }

//        // Additional properties for frontend compatibility
//        public decimal AdvanceAmount { get; set; } = 0;
//        public decimal DebtAmount { get; set; } = 0;
//        public decimal QualityBonus { get; set; } = 0;
//        public decimal LoyaltyBonus { get; set; } = 0;
//    }
//}
