using System;
using System.ComponentModel.DataAnnotations;
using test6API.Models;

namespace TFSMS_app_backend.Models
{
    public class Harvest
    {
        [Key]
        public int HarvestId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public DateTime Time { get; set; }

        [Required]
        public double SupperLeafWeight { get; set; }

        [Required]
        public double NormalLeafWeight { get; set; }

        [Required]
        public TransportMethodType TransportMethod { get; set; }

        [Required]
        public PaymentMethodType PaymentMethod { get; set; }

        public string Address { get; set; } = string.Empty;

        public string Status { get; set; } = "Pending";

        // Foreign key to Grower
        public int GrowerAccountId { get; set; }

        public GrowerCreateAccount? Grower { get; set; }

        // Optional bank details
        public string? BankName { get; set; }

        public string? BankAccountNumber { get; set; }
    }
}
