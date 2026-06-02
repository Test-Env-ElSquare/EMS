namespace Shared.Dtos
{
    public class TransformerSummaryDto
    {
        public decimal TotalEnergy { get; set; }
        public int TotalTransformers { get; set; }
        public int OnlineTransformers { get; set; }
        public int OfflineTransformers { get; set; }
        public decimal AvgPowerFactor { get; set; }
    }
}
