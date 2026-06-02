using Domain.Models.Definitions;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.RealTime
{
    [Table(name: "MachineLoads", Schema = "RealTime")]
    public class MachineLoad
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? MaterialUID { get; set; } // عني كل خامة ليها UID بيتتبع في السيستم)
        public int? MachineId { get; set; }
        public Machine? Machine { get; set; }
        public decimal? Weight { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.Now;
        public int? SKU { get; set; }
        public string? JobOrderId { get; set; }
        public string? BatchId { get; set; }
        public string? SKUCode { get; set; }
        public string? SKUName { get; set; }
        public DateTime ShiftStartTime { get; set; }
        public int? LineId { get; set; }
        public DateTime? ActivationDate { get; set; }  // وقت دخول material للماكينة
        public DateTime? FinishingDate { get; set; } // وقت خروجها
        public bool? IsPending { get; set; }
        public int? Count { get; set; }
    }
}
