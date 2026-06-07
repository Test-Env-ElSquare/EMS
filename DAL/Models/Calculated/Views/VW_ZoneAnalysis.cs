namespace DAL.Models.Calculated.Views
{
    public class VW_ZoneAnalysis
    {
        public int ZoneId { get; set; }
        public string ZoneName { get; set; }
        public int? TransformerId { get; set; }
        public DateTime ShiftStartTime { get; set; }
        public decimal EnergyConsumption { get; set; }
    }
}
