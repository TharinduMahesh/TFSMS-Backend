using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    public class Claim
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Season { get; set; }

        [Required]
        public string GardenMark { get; set; }

        [Required]
        public string Invoice { get; set; }

        [Required]
        public DateTime ReturnDate { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Kilos returned must be a positive number")]
        public int KilosReturned { get; set; }

        [Required]
        public string Status { get; set; }
    }
}