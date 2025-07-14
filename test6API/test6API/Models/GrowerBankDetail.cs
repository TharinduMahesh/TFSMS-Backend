using System.ComponentModel.DataAnnotations;

namespace test6API.Models
{
    public class GrowerBankDetail
    {
        [Key]
        public int BankDetailId { get; set; }
        [Required]
        public string BankName { get; set; }
        [Required]
        public string BranchName { get; set; }
        [Required]
        public string AccountHolderName { get; set; }
        [Required]
        public string GrowerEmail { get; set; }
    }
}
