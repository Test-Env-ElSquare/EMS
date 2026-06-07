using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Calculated.Historical
{
    [Table("LineHourlyAnalysis", Schema = "Calculated")]

    public class LineHourlyAnalysis
    {
        public int FactoryId { get; set; }

        public int LineId { get; set; }
        public string LineName { get; set; }
        public int? ZoneId { get; set; }
        public DateTime ShiftStartTime { get; set; }
        public DateTime HourStartTime { get; set; }
        public decimal Availability { get; set; }
        public decimal AdjustedAvailability { get; set; }
        public decimal EnergyConsumption { get; set; }
    }
}
