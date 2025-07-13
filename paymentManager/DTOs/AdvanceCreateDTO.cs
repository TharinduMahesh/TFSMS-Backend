using System.ComponentModel.DataAnnotations;

namespace paymentManager.DTOs
{
    public class AdvanceCreateDTO
    {
        [Required]
        public int SupplierId { get; set; }

        [Required]
        [StringLength(50)]
        public string AdvanceType { get; set; }

        [Required]
        [StringLength(200)]
        public string Purpose { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Advance amount must be greater than 0")]
        public decimal AdvanceAmount { get; set; }

        [Required]
        public DateTime IssueDate { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Recovered amount must be 0 or greater")]
        public decimal RecoveredAmount { get; set; } = 0;
    }
}
