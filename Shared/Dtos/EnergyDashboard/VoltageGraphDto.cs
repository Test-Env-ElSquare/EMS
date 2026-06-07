namespace Shared.Dtos.EnergyDashboard
{
    public class VoltageGraphDto
    {
        public int FactoryId { get; set; }
        public DateTime HourStartTime { get; set; }
        public decimal AvgVoltage { get; set; }
    }
}
