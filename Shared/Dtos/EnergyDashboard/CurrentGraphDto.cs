namespace Shared.Dtos.EnergyDashboard
{
    public class CurrentGraphDto
    {
        public int FactoryId { get; set; }
        public DateTime HourStartTime { get; set; }
        public decimal AvgCurrent { get; set; }
    }
}
