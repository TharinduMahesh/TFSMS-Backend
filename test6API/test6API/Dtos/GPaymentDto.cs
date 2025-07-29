namespace test6API.Dtos
{
    public class GPaymentDto
    {
        public int GrowerOrderId { get; set; }
        public string CollectorFirstName { get; set; }
        public string CollectorLastName { get; set; }
        public string CollectorCity { get; set; }
        public decimal GrossPayment { get; set; }
    }
}
