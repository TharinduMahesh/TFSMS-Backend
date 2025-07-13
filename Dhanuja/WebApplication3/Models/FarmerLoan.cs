// Models/FarmerLoan.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    public class FarmerLoan
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int FarmerId { get; set; }

        [Required]
        public string FarmerName { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal LoanAmount { get; set; }

        [Required]
        public DateTime LoanDate { get; set; }

        [Required]
        [Range(1, 120)] // Duration in months
        public int LoanTerm { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Repayments { get; set; }

        public decimal OutstandingBalance { get; set; }

        public decimal Incentives { get; set; }

        [Required]
        public string LoanStatus { get; set; } // Active, Paid Off, Defaulted
    }
}
