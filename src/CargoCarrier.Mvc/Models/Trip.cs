using System;

namespace CargoCarrier.Mvc.Models
{
    public class Trip
    {
        public int Id { get; set; }
        public int ParcelSize { get; set; }
        public string? LicensePlate { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
