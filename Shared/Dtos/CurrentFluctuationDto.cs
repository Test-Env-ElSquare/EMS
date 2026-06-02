namespace Shared.Dtos
{
    public class CurrentFluctuationDto
    {
        public int FactoryId { get; set; }
        public DateTime HourStartTime { get; set; }
        public decimal AvgCurrent { get; set; }
    }
}
