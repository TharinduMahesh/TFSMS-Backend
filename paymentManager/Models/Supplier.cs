using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace paymentManager.Models
{
    public class Supplier
    {
        [Key]
        public int SupplierId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(20)]
        public string Contact { get; set; } // Matches your existing service

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(200)]
        public string Area { get; set; } // Matches your existing service

        public bool IsActive { get; set; } = true;

        public DateTime JoinDate { get; set; } = DateTime.Now; // Matches your existing service

        // Navigation properties
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public virtual ICollection<Advance> Advances { get; set; } = new List<Advance>();
        public virtual ICollection<Debt> Debts { get; set; } = new List<Debt>();
        public virtual ICollection<Incentive> Incentives { get; set; } = new List<Incentive>();
        public virtual ICollection<GreenLeafData> GreenLeafData { get; set; } = new List<GreenLeafData>();
    }
}
