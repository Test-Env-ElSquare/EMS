namespace Shared.Dtos
{
    //public class TransformerSummaryDto
    //{
    //    public decimal TotalEnergy { get; set; }
    //    public int TotalTransformers { get; set; }
    //    public int OnlineTransformers { get; set; }
    //    public int OfflineTransformers { get; set; }
    //    public decimal AvgPowerFactor { get; set; }
    //}

    public class TransformerSummaryDto
    {
        public decimal TotalEnergy { get; set; }
        public EntityCountDto Transformers { get; set; }
        public EntityCountDto Zones { get; set; }
        public EntityCountDto Lines { get; set; }
        public decimal AvgPowerFactor { get; set; }
        public string Level { get; set; }
    }

    public class EntityCountDto
    {
        public int TotalCount { get; set; }
    }

    public class OnlineStatusDto
    {
        public EntityStatusDto Transformers { get; set; }
        public EntityStatusDto Zones { get; set; }
        public EntityStatusDto Lines { get; set; }
        public string Level { get; set; }
    }

    public class EntityStatusDto
    {
        public int OnlineCount { get; set; }
        public int OfflineCount { get; set; }
    }
}
