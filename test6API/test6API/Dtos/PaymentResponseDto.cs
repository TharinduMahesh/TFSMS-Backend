namespace test6API.Dtos
{
    public class PaymentResponseDto
    {
        public decimal TotalAmount { get; set; }
        public List<PaymentItemDto> Payments { get; set; } = new();
    }

    public class PaymentItemDto
    {
        public string RefNumber { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime PaymentTime { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
    }

}
