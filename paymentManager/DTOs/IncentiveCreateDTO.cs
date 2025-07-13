namespace paymentManager.DTOs
{
    public class IncentiveCreateDTO
    {
        public int SupplierId { get; set; }
        public decimal QualityBonus { get; set; }
        public decimal LoyaltyBonus { get; set; }
        public string Month { get; set; }
    }
}
