using Domain.Models.Definitions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DAL.Models.Definitions
{
    [Table(name: "Zones", Schema = "Definitions")]

    public class Zone : BaseEntity
    {
        public string Name { get; set; }
        public int FactoryId { get; set; }
        public int? TransformerId { get; set; }

        // Navigation Property
        [JsonIgnore]
        public Factory? Factory { get; set; }
        [JsonIgnore]
        public ICollection<Line>? Lines { get; set; }
        [JsonIgnore]
        public Transformer? Transformer { get; set; }  // ✅ موجودة؟
        public decimal? RatioFromParent { get; set; }
    }
}
