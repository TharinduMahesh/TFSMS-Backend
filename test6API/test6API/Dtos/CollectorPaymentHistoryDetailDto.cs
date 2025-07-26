namespace test6API.Dtos
{
    public class CollectorPaymentHistoryDetailDto
    {
        public int RefNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string NIC { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal Amount { get; set; } // Added this property to fix the error  
    }
}
