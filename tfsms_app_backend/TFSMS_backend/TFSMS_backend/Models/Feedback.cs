namespace TFSMS_app_backend.Models;

    public class Feedback
    {
        public Guid Id { get; set; }

        public double Rating { get; set; }

        public string Tags { get; set; } = string.Empty;

        public string Comment { get; set; } = string.Empty;
    }

