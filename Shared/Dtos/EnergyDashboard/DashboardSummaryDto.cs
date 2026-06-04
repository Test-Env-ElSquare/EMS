namespace Shared.Dtos.EnergyDashboard
{
    public class DashboardSummaryDto
    {
        public decimal TotalEnergy { get; set; }

        public CountStatusDto Transformers { get; set; } = new();
        public CountStatusDto Zones { get; set; } = new();
        public CountStatusDto Lines { get; set; } = new();

        public decimal AvgPowerFactor { get; set; }

        public string Level { get; set; } = "Main";
    }

    public class CountStatusDto
    {
        public int TotalCount { get; set; }
        public int OnlineCount { get; set; }
        public int OfflineCount { get; set; }
    }
}
