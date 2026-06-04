namespace Shared.Dtos.EnergyDashboard
{
    public class DashboardGraphPointDto
    {
        public int FactoryId { get; set; }

        public int? TransformerId { get; set; }
        public int? ZoneId { get; set; }

        public DateTime HourStartTime { get; set; }

        public decimal Value { get; set; }
    }
}
