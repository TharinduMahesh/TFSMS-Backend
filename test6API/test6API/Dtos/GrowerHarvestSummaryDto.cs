using test6API.Models;

namespace test6API.Dtos
{
    public class GrowerHarvestSummaryDto
    {
        public decimal TotalSuperTeaQuantity { get; set; }
        public decimal TotalGreenTeaQuantity { get; set; }
        public decimal TotalHarvest => TotalSuperTeaQuantity + TotalGreenTeaQuantity;

        public List<GrowerOrder> Orders { get; set; } = new();
    }
}
