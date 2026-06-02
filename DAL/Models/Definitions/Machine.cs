using DAL.Models;
using DAL.Models.RealTime;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Models.Definitions
{
    [Table(name: "Machines", Schema = "Definitions")]
    public class Machine : BaseEntity
    {
        public string Name { get; set; }
        public string Faunctionality { get; set; }
        public decimal? RatedSpeed { get; set; }

        [JsonIgnore]
        public Line? Line { get; set; }
        public int LineId { get; set; }

        [JsonIgnore]
        public List<MachineLoad>? MachineLoad { get; set; }

        public string? UID { get; set; }
        public string? UnitOfSpeed { get; set; }
        public string? Building { get; set; }
        public string? Floor { get; set; }
        public string? Room { get; set; }
        public bool? IsDominant { get; set; }

        // Relation: Many-to-Many through MachineShift
        public ICollection<MachineShift> MachineShifts { get; set; } = new List<MachineShift>();
    }
}
