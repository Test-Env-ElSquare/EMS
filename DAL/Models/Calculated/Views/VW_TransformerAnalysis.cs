namespace DAL.Models.Calculated.Views
{
    public class VW_TransformerAnalysis
    {
        public int TransformerId { get; set; }
        public string TransformerName { get; set; }
        public int FactoryId { get; set; }
        public DateTime ShiftStartTime { get; set; }
        public decimal TotalEnergyConsumption { get; set; }
        public string Status { get; set; }
        public decimal PowerFactor { get; set; }
        public decimal Voltage { get; set; }
        public decimal Current { get; set; }
        public decimal Hermonics { get; set; }

    }
}
