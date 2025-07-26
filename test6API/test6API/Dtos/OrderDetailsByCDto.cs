namespace test6API.Dtos
{
    public class OrderDetailsByCDto
    {
        public int GrowerOrderId { get; set; }
        public decimal SuperTeaQuantity { get; set; }
        public decimal GreenTeaQuantity { get; set; }
        public decimal TotalTea => SuperTeaQuantity + GreenTeaQuantity;
        public DateTime PlaceDate { get; set; }
        public string PaymentMethod { get; set; }
    }
}
