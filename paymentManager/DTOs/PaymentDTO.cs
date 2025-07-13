namespace paymentManager.DTOs
{
    public class PaymentDTO
    {
        public int PaymentId { get; set; }
        public int SupplierId { get; set; }
        public decimal LeafWeight { get; set; }
        public decimal Rate { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal AdvanceDeduction { get; set; }
        public decimal DebtDeduction { get; set; }
        public decimal IncentiveAddition { get; set; }
        public decimal NetAmount { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime PaymentDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        // Navigation properties for frontend - using your existing SupplierDTO
        public SupplierDTO Supplier { get; set; }
        public List<PaymentReceiptDTO> Receipts { get; set; } = new List<PaymentReceiptDTO>();
    }

    public class PaymentReceiptDTO
    {
        public int ReceiptId { get; set; }
        public int PaymentId { get; set; }
        public string ReceiptNumber { get; set; }
        public DateTime GeneratedDate { get; set; }
    }
}
