namespace DAL.Models.Calculated.Views
{
    public class VW_LineHourlyAnalysis
    {
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
