public class PendingPaymentDto
{
    public int PaymentId { get; set; }
    public int GrowerOrderId { get; set; }
    public string GrowerName { get; set; }
    public string GrowerCity { get; set; }
    public decimal NetPayment { get; set; }
}