namespace Shared.Dtos
{
    public class TopEnergyConsumerDto
    {
        public int Rank { get; set; }
        public int TransformerId { get; set; }
        public string TransformerName { get; set; }
        public decimal TotalEnergyConsumption { get; set; }
        public decimal PercentageOfTotal { get; set; }
    }
}
