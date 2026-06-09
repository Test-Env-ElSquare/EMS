using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Calculated.Historical
{
    [Table("TransformerHourlyAnalysisCurrent", Schema = "Calculated")]
    public class TransformerHourlyAnalysisCurrent
    {
        public int TransformerId { get; set; }
        public string TransformerName { get; set; }
        public int FactoryId { get; set; }
        public DateTime ShiftStartTime { get; set; }
        public DateTime HourStartTime { get; set; }
        public decimal TotalEnergyConsumption { get; set; }
        public string Status { get; set; }
        public decimal PowerFactor { get; set; }
        public decimal Voltage { get; set; }
        public decimal Current { get; set; }
        public decimal Hermonics { get; set; }
        public decimal AvgTHDv { get; set; }
        public decimal AvgTHDi { get; set; }
    }
}
