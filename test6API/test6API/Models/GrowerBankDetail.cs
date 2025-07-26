using System.ComponentModel.DataAnnotations;

namespace test6API.Models
{
    public class GrowerBankDetail
    {
        [Key] 
        public int GrowerDetailId { get; set; }

        [Required]
        [MaxLength(100)]
        public string BankName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Branch { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string AccountHolderName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string AccountNumber { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string GrowerEmail { get; set; } = string.Empty;
    }
}
