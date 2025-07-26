using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace test6API.Models
{
    public class CollectorPayment
    {
        [Key]
        public int RefNumber { get; set; }
        public decimal Amount { get; set; }
        public string GrowerNIC { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string CollectorEmail { get; set; }

        [ForeignKey("GrowerNIC")]
        public GrowerCreateAccount Grower { get; set; }
    }
}
