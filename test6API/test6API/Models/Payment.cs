
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace test6API.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        // Foreign key to the GrowerOrder
        [Required]
        public int GrowerOrderId { get; set; }

        [ForeignKey("GrowerOrderId")]
        public virtual GrowerOrder GrowerOrder { get; set; }

        [Required]
        public string GrowerEmail { get; set; }

        [Required]
        public string CollectorEmail { get; set; }

        public decimal GrossPayment { get; set; }

        public DateTime PaymentDate { get; set; }

        [StringLength(50)]
        public string PaymentStatus { get; set; } = "Pending";
    }
}

