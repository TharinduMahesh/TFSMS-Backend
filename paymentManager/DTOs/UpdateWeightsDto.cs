namespace paymentManager.DTOs;

public class UpdateWeightsDto
{
    public double SupperLeafWeight { get; set; }
    public double NormalLeafWeight { get; set; }
    public string Status { get; set; } = "Weighed";
}
