using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Calculated.Historical
{
    [Table("ZoneAnalysis", Schema = "Calculated")]
    public class ZoneAnalysis
    {
        public int ZoneId { get; set; }
        public int FactoryId { get; set; }
        public string ZoneName { get; set; }
        public int? TransformerId { get; set; }
        public DateTime ShiftStartTime { get; set; }
        public decimal EnergyConsumption { get; set; }
        public string Status { get; set; }

    }
}
