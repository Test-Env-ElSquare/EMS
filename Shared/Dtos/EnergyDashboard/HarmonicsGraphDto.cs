namespace Shared.Dtos.EnergyDashboard
{
    public class HarmonicsGraphDto
    {
        public int FactoryId { get; set; }
        public DateTime HourStartTime { get; set; }
        public decimal AvgHarmonics { get; set; }
    }
}
