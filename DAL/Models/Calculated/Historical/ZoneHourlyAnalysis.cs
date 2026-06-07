using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Calculated.Historical
{
    [Table("ZoneHourlyAnalysis", Schema = "Calculated")]

    public class ZoneHourlyAnalysis
    {
        public int ZoneId { get; set; }
        public string ZoneName { get; set; }
        public int? TransformerId { get; set; }
        public DateTime ShiftStartTime { get; set; }
        public DateTime HourStartTime { get; set; }
        public decimal EnergyConsumption { get; set; }
    }
}
