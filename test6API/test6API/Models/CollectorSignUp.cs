using System.ComponentModel.DataAnnotations;

namespace test6API.Models
{
    public class CollectorSignUp
    {
        [Key]
        public int CollectorId { get; set; }

        [Required, EmailAddress]
        public string CollectorEmail { get; set; }

        [Required]
        public string CollectorPassword { get; set; }

        public DateTime CollectorSignUpdate { get; set; } = DateTime.Now;

    }
}
