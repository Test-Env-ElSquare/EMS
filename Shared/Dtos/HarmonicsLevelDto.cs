namespace Shared.Dtos
{
    public class HarmonicsLevelDto
    {
        public int FactoryId { get; set; }
        public DateTime HourStartTime { get; set; }
        public decimal AvgTHDv { get; set; }
        public decimal AvgTHDi { get; set; }

    }
}
