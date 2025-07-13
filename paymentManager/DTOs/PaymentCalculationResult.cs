//using System;
//using System.Text.Json.Serialization;

//namespace paymentManager.DTOs
//{
//    public class PaymentCalculationResult
//    {
//        [JsonPropertyName("supplierId")]
//        public int SupplierId { get; set; }

//        [JsonPropertyName("leafWeight")]
//        public decimal LeafWeight { get; set; }

//        [JsonPropertyName("rate")]
//        public decimal Rate { get; set; }

//        [JsonPropertyName("grossAmount")]
//        public decimal GrossAmount { get; set; }

//        [JsonPropertyName("advanceDeduction")]
//        public decimal AdvanceDeduction { get; set; }

//        [JsonPropertyName("debtDeduction")]
//        public decimal DebtDeduction { get; set; }

//        [JsonPropertyName("incentiveAddition")]
//        public decimal IncentiveAddition { get; set; }

//        [JsonPropertyName("netAmount")]
//        public decimal NetAmount { get; set; }

//        [JsonPropertyName("calculatedAt")]
//        public DateTime CalculatedAt { get; set; } = DateTime.Now;
//    }
//}
