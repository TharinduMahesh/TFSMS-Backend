using System.ComponentModel.DataAnnotations;

namespace paymentManager.Models
{
    public class TeaReturn
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Season { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string GardenMark { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string InvoiceNumber { get; set; } = string.Empty;

        [Required]
        public DateTime ReturnDate { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Kilos returned must be greater than 0")]
        public decimal KilosReturned { get; set; }

        [Required]
        [StringLength(500)]
        public string Reason { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
