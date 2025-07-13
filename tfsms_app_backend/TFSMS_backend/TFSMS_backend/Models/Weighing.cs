using System.ComponentModel.DataAnnotations;

namespace TFSMS_app_backend.Models;

public class Weighing
{
    [Key]
    public int HarvestId { get; set; }

    [Required]
    public string SupplierId { get; set; } = string.Empty;

    [Required]
    public double SupperLeafWeight { get; set; }

    [Required]
    public double NormalLeafWeight { get; set; }

    [Required]
    public PaymentMethodType PaymentMethod { get; set; }

}
