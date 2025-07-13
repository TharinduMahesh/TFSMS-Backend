using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    public class TeaPackingLedger
    {
        [Key]
        public string SaleId { get; set; }
        public string BuyerName { get; set; }
        public int KilosSold { get; set; }
        public decimal SoldPriceKg { get; set; }
        public string TransactionType { get; set; }
        public DateTime SaleDate { get; set; }
        public string Status { get; set; }
    }
}