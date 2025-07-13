// WebApplication3/Models/ClaimEntry.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    public class ClaimEntry
    {
        [Key]
        public int Id { get; set; }

        public string ClaimType { get; set; } = string.Empty; // Ensure initialization
        public string InvoiceNumber { get; set; } = string.Empty; // Ensure initialization
        public int Quantity { get; set; }
        public DateTime? ReturnDate { get; set; } // FIX: Make nullable (DateTime?)
        public string Remark { get; set; } = string.Empty; // Ensure initialization
    }
}