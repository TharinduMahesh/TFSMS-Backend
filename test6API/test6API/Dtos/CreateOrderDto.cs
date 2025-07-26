namespace test6API.DTOs
{
    public class CreateOrderDto
    {
        public decimal SuperTeaQuantity { get; set; }
        public decimal GreenTeaQuantity { get; set; }
        public string TransportMethod { get; set; }
        public string PaymentMethod { get; set; }
        public string GrowerEmail { get; set; }
    }
}
