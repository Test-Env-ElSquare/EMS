using DAL.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Definitions
{
    [Table(name: "MachineShifts", Schema = "Definitions")]
    public class MachineShift : BaseEntity
    {
        public int MachineId { get; set; }
        public int? SKUId { get; set; }
        public DateTime ShiftDate { get; set; }
        public int ShiftNumber { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int? PlannedQty { get; set; }
        public int? ActualQty { get; set; }
        public int? RejectedQty { get; set; }
        public DateTime? ScheduledAt { get; set; }

        [ForeignKey(nameof(MachineId))]
        public Machine Machine { get; set; }

        [ForeignKey(nameof(SKUId))]
        public SKU? SKU { get; set; }
    }

}
