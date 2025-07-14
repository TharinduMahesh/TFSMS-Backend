using System.ComponentModel.DataAnnotations;

namespace test6API.Models
{
    public class CollectorCreateAccount
    {
        [Key]
        public int CollectorAccountId { get; set; }

        [Required]
        public string CollectorFirstName { get; set; }

        [Required]
        public string CollectorLastName { get; set; }

        [Required]
        public string CollectorNIC { get; set; }

        [Required]
        public string CollectorAddressLine1 { get; set; }

        public string CollectorAddressLine2 { get; set; }

        [Required]
        public string CollectorCity { get; set; }

        public string CollectorPostalCode { get; set; }

        public string CollectorGender { get; set; }

        public DateTime? CollectorDOB { get; set; }

        [Required]
        public string CollectorPhoneNum { get; set; }
        [Required]
        public string CollectorVehicleNum { get; set; }

        [Required]
        public string CollectorEmail { get; set; }
    }
}
