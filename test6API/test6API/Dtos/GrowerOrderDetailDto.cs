namespace test6API.Dtos
{
    public class GrowerOrderDetailDto
    {
        public decimal TotalAmount { get; set; }
        public decimal SuperTeaQuantity { get; set; }
        public decimal GreenTeaQuantity { get; set; }
        public string CollectorFirstName { get; set; }
        public string CollectorLastName { get; set; }
        public string CollectorPhoneNum { get; set; }
        public string? CollectorVehicleNum { get; set; }
    }
}
