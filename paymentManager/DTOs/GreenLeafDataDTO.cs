namespace paymentManager.DTOs
{
    public class GreenLeafDataDTO
    {
        public int LeafDataId { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public decimal LeafWeight { get; set; }
        public string QualityGrade { get; set; }
        public decimal MoistureLevel { get; set; }
        public DateTime CollectionDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}