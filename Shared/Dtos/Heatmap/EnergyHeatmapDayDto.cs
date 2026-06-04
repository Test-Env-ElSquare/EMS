namespace Shared.Dtos.Heatmap
{
    public class EnergyHeatmapDayDto
    {
        public DateTime Date { get; set; }

        public List<EnergyHeatmapDto> Items { get; set; } = new();
    }
}
