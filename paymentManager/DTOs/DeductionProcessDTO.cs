namespace paymentManager.DTOs
{
    public class DeductionProcessDTO
    {
        public int SupplierId { get; set; }
        public decimal DeductionAmount { get; set; }
        public int PaymentId { get; set; }
    }
}
