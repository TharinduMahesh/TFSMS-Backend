using System.ComponentModel.DataAnnotations;

namespace paymentManager.Models
{
    public class Invoice
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string InvoiceNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Season { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string GardenMark { get; set; } = string.Empty;

        public DateTime InvoiceDate { get; set; }
        public decimal TotalAmount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
