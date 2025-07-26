namespace test6API.Dtos
{
    public class OrderDetailsDto
    {
        public int GrowerOrderId { get; set; }
        public decimal SuperTeaQuantity { get; set; }
        public decimal GreenTeaQuantity { get; set; }
        public decimal TotalTea => SuperTeaQuantity + GreenTeaQuantity;
        public DateTime PlaceDate { get; set; }
        public string PaymentMethod { get; set; }

        public string GrowerName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string NIC { get; set; }
        public string PhoneNumber { get; set; }
    }
}
