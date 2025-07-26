namespace test6API.Dtos
{
    public class CollectorPaymentHistoryDto
    {
        public int RefNumber { get; set; }
        public decimal Amount { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string GrowerName { get; set; }
    }
}
