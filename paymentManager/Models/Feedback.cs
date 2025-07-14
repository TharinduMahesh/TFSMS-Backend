using System.ComponentModel.DataAnnotations;

namespace paymentManager.Models
{
    public class Feedback
    {
        public int Id { get; set; }

        [Range(0, 5)]
        public double Rating { get; set; }

        [MaxLength(255)]
        public string Tags { get; set; } = string.Empty;

        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
