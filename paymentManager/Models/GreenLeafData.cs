// Models/GreenLeafData.cs
// This should match your existing GreenLeafData model structure
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace paymentManager.Models
{
    public class GreenLeafData
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SupplierId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Weight { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation property to Supplier
        public virtual Supplier Supplier { get; set; }
    }
}
