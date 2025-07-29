namespace test6API.Models.DTOs
{
    public class GrowerPaidOrderDto
    {
        public int GrowerOrderId { get; set; }
        public string CollectorName { get; set; }
        public string CollectorCity { get; set; }
        public decimal NetPayment { get; set; }
    }
}
