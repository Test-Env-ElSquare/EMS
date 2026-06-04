using Domain.Models.Definitions;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.RealTime
{
    [Table(name: "Signals", Schema = "RealTime")]
    public class Signal
    {
        public int Id { get; set; }
        public string MachineName { get; set; }
        public int State { get; set; }
        public int Count { get; set; }
        public int Qreject { get; set; }
        public decimal Speed { get; set; }
        public string? StateType { get; set; }
        public string? StateReason { get; set; }
        public string SKU { get; set; }
        public int CountDiff { get; set; }
        public int QrejectDiff { get; set; }
        public DateTime TimeStamp { get; set; }
        public DateTime ShiftStartTime { get; set; }
        public Machine? Machine { get; set; }
        public int? MachineId { get; set; }
        public int? FactoryId { get; set; }
        public int? LineId { get; set; }
        public int Fault { get; set; }
        public string? Functionality { get; set; }
    }
}
