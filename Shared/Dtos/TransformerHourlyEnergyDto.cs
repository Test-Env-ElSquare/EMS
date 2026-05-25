namespace Shared.Dtos
{
    public class TransformerHourlyEnergyDto
    {
        public int FactoryId { get; set; }
        public int TransformerId { get; set; }
        public string TransformerName { get; set; }
        public DateTime HourStartTime { get; set; }
        public decimal TotalEnergyConsumption { get; set; }
    }
}
