namespace DAL.Models.Threshold
{
    public class EnergyHeatmapThreshold
    {
        public int Id { get; set; }

        public string Level { get; set; } = string.Empty;
        // Transformer, Zone, Line

        public decimal LowFrom { get; set; }
        public decimal LowTo { get; set; }

        public decimal MediumFrom { get; set; }
        public decimal MediumTo { get; set; }

        public decimal HighFrom { get; set; }
        public decimal HighTo { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
