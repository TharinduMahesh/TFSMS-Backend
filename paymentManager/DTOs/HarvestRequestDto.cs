namespace paymentManager.DTOs;

public class HarvestRequestDto
{
    public DateTime Date { get; set; }
    public DateTime Time { get; set; }

    public double SupperLeafWeight { get; set; }
    public double NormalLeafWeight { get; set; }

    public string TransportMethod { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public int GrowerAccountId { get; set; }
}
