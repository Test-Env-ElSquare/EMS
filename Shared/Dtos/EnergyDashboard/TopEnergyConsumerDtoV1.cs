namespace Shared.Dtos.EnergyDashboard
{
    public class TopEnergyConsumerDtoV1
    {
        public int Rank { get; set; }

        public string Level { get; set; } = string.Empty;

        public int? TransformerId { get; set; }
        public string? TransformerName { get; set; }

        public int? ZoneId { get; set; }
        public string? ZoneName { get; set; }

        public int? LineId { get; set; }
        public string? LineName { get; set; }

        public decimal Energy { get; set; }

        public decimal PercentageOfTotal { get; set; }
    }
}
