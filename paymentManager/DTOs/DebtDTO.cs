namespace paymentManager.DTOs
{
    public class DebtDTO
    {
        public int DebtId { get; set; }
        public int SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public decimal BalanceAmount { get; set; }
        public decimal DeductionsMade { get; set; }
        public string Description { get; set; }
        public string DebtType { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DeductionPercentage { get; set; }
        public string Status { get; set; }
        public DateTime IssueDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
