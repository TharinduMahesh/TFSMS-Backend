using test6API.Models;

namespace test6API.Dtos
{
    public class FertilizerDto
    {
        public string RefNumber { get; set; }
        public DateTime Date { get; set; }
        public decimal FertilizerAmount { get; set; }
    }
}
