using System.ComponentModel.DataAnnotations;

namespace paymentManager.DTOs
{
    public class CreateTeaReturnDto
    {
        [Required]
        public string Season { get; set; } = string.Empty;

        [Required]
        public string GardenMark { get; set; } = string.Empty;

        [Required]
        public string InvoiceNumber { get; set; } = string.Empty;

        [Required]
        public DateTime ReturnDate { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal KilosReturned { get; set; }

        [Required]
        public string Reason { get; set; } = string.Empty;
    }

    public class TeaReturnDto
    {
        public int Id { get; set; }
        public string Season { get; set; } = string.Empty;
        public string GardenMark { get; set; } = string.Empty;
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateTime ReturnDate { get; set; }
        public decimal KilosReturned { get; set; }
        public string Reason { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
