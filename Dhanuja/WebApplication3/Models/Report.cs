// Models/Report.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    public class Report
    {
        [Key]
        public int Id { get; set; }
        public string DispatchID { get; set; }
        public string Yield { get; set; }
        public int BagCount { get; set; }
        public string VehicleNumber { get; set; }
        public string DriverNIC { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
    }
}
