namespace test6API.Models
{
    public class Fertilizer
    {
        public int Id { get; set; }
        public int GrowerId { get; set; }
        public string RefNumber { get; set; }
        public DateTime Date { get; set; }
        public decimal FertilizerAmount { get; set; }
    }
}
