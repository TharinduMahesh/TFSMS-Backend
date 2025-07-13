using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace paymentManager.Models
{
    public class PaymentHistory
    {
        [Key]
        public int HistoryId { get; set; }

        [Required]
        public int PaymentId { get; set; }

        [Required]
        [StringLength(50)]
        public string Action { get; set; }

        public DateTime ActionDate { get; set; } = DateTime.Now;

        [StringLength(50)]
        public string ActionBy { get; set; }

        public string Details { get; set; }

        // Navigation property
        [ForeignKey("PaymentId")]
        public virtual Payment Payment { get; set; }
    }
}
