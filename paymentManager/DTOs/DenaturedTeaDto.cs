using System.ComponentModel.DataAnnotations;

namespace paymentManager.DTOs
{
    public class CreateDenaturedTeaDto
    {
        [Required]
        public string TeaGrade { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal QuantityKg { get; set; }

        [Required]
        public string Reason { get; set; } = string.Empty;

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string InvoiceNumber { get; set; } = string.Empty;
    }

    public class DenaturedTeaDto
    {
        public int Id { get; set; }
        public string TeaGrade { get; set; } = string.Empty;
        public decimal QuantityKg { get; set; }
        public string Reason { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
