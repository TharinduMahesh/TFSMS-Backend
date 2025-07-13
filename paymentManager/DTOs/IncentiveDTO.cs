namespace paymentManager.DTOs
{
    public class IncentiveDTO
    {
        public int IncentiveId { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public decimal QualityBonus { get; set; }
        public decimal LoyaltyBonus { get; set; }
        public decimal TotalAmount { get; set; }
        public string Month { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
