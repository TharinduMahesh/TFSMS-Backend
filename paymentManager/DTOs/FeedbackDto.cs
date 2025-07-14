namespace paymentManager.DTOs
{
    public class FeedbackDto
    {
        public double Rating { get; set; }
        public string Tags { get; set; } = string.Empty;
        public string? Comment { get; set; }
    }
}

