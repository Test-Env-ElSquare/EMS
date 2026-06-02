namespace Shared.Dtos
{
    public class VoltageStabilityDto
    {
        public int FactoryId { get; set; }
        public DateTime HourStartTime { get; set; }
        public decimal AvgVoltage { get; set; }
    }
}
