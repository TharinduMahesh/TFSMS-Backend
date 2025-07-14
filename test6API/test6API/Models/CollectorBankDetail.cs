using System.ComponentModel.DataAnnotations;

namespace test6API.Models
{
    public class CollectorBankDetail
    {
        [Key]
        public int CollectorDetailId { get; set; }
        [Required]
        public string CollectorName { get; set; }
        [Required]
        public string BranchName { get; set; }
        [Required]
        public string AccountHolderName { get; set; }
        [Required]
        public string CollectorEmail { get; set; }
    }
}
