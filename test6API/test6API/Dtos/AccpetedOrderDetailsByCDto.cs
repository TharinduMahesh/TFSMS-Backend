using System.ComponentModel.DataAnnotations;

namespace test6API.Dtos
{
    public class AccpetedOrderDetailsByCDto
    {
        public int GrowerOrderId { get; set; }
        public string  CollectorFirstName { get; set; }
        public string CollectorSecondName { get; set; }
        public decimal SuperTeaQuantity { get; set; }
        public decimal GreenTeaQuantity { get; set; }
        public decimal TotalTea => SuperTeaQuantity + GreenTeaQuantity;

        public string PaymentMethod { get; set; }
        public DateTime PlaceDate { get; set; }
        public string CollectorAddressLine1 { get; set; }

        public string CollectorAddressLine2 { get; set; }

        public string CollectorCity { get; set; }

        public string CollectorPostalCode { get; set; }

        public string CollectorPhoneNum { get; set; }

        public string CollectorVehicleNum { get; set; }

    }
}

