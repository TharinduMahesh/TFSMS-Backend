namespace paymentManager.DTOs
{
    public class AdvanceDTO
    {
        public int AdvanceId { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public decimal AdvanceAmount { get; set; }
        public decimal BalanceAmount { get; set; }
        public string Purpose { get; set; }
        public string AdvanceType { get; set; }
        public decimal RecoveredAmount { get; set; }
        public decimal RecoveryPercentage { get; set; }
        public string Status { get; set; }
        public DateTime IssueDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}