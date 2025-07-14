namespace test6API.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public decimal Amount { get; set; }
        public string RefNumber { get; set; } = string.Empty;
        public DateTime PaymentTime { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string GrowerEmail { get; set; } = string.Empty;
    }
}
