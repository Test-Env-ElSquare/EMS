namespace Shared.Dtos.Heatmap
{
    public class EnergyHeatmapDto
    {
        public DateTime Date { get; set; }

        public int ItemId { get; set; }

        public string ItemName { get; set; } = string.Empty;

        public string Level { get; set; } = string.Empty;

        public decimal Energy { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}
