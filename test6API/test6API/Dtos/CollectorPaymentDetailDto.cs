namespace test6API.Dtos
{
    public class CollectorPaymentDetailDto
    {
        public int RefNumber { get; set; }
        public decimal Amount { get; set; }
        public string? FirstName { get; set; }      // Change to nullable
        public string? LastName { get; set; }       // Change to nullable
        public string? AddressLine1 { get; set; }   // Change to nullable
        public string? AddressLine2 { get; set; }
        public string? City { get; set; }           // Change to nullable
        public string? PostalCode { get; set; }
        public string? NIC { get; set; }            // Change to nullable
        public string? PhoneNumber { get; set; }
        public string? PaymentStatus { get; set; }  // Change to nullable
        public DateTime? PaymentDate { get; set; }
    }
}