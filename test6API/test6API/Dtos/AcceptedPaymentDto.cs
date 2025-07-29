namespace test6API.Models.DTOs
{
    public class AcceptedPaymentDto
    {
        public int PaymentId { get; set; }
        public int GrowerOrderId { get; set; }
        public string GrowerName { get; set; }
        public string GrowerCity { get; set; }
        public decimal NetPayment { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
