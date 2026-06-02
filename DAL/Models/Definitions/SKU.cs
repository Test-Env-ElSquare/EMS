using DAL.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Models.Definitions
{
    [Table(name: "SKUs", Schema = "Definitions")]
    public class SKU : BaseEntity
    {
        public string Name { get; set; }
        public int TechnicalSKU { get; set; }
        public string? BatchId { get; set; }
        public string? JobOrderId { get; set; }

        [JsonIgnore]
        public Line Line { get; set; }
        public int? LineId { get; set; }
        public int? PackSize { get; set; }
        public int? CartonSize { get; set; }
        public int? PallSize { get; set; }
        public decimal Size { get; set; }
        public string Unit { get; set; }
        public string? SkuCode { get; set; }

        // Relation: One SKU -> Many Shifts
        public ICollection<MachineShift> MachineShifts { get; set; } = new List<MachineShift>();

    }
}
