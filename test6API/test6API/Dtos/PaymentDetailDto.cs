public class PaymentDetailDto
{
    public decimal TotalAmount { get; set; }
    public decimal SuperTeaQuantity { get; set; }
    public decimal GreenTeaQuantity { get; set; }
    public string GrowerName { get; set; }
    public string GrowerAddressLine1 { get; set; }
    public string? GrowerAddressLine2 { get; set; }
    public string GrowerCity { get; set; }
    public string? GrowerPostalCode { get; set; }
    public string GrowerPhoneNum { get; set; }
}