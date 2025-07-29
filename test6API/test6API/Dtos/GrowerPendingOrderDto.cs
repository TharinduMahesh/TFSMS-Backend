namespace test6API.Dtos
{
    public class GrowerPendingOrderDto
    {
        public int GrowerOrderId { get; set; }
        public string CollectorName { get; set; }
        public string CollectorCity { get; set; }
        public decimal NetPayment { get; set; }
    }
}
