using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Calculated.Historical
{
    [Table("LineAnalysis", Schema = "Calculated")]
    public class LineAnalysis
    {
        public int LineId { get; set; }
        public string LineName { get; set; }
        public int? ZoneId { get; set; }
        public DateTime ShiftStartTime { get; set; }
        public decimal Availability { get; set; }
        public decimal AdjustedAvailability { get; set; }
        public decimal EnergyConsumption { get; set; }
    }
}
