using DAL.Models.Definitions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Models.Definitions
{
    [Table(name: "Lines", Schema = "Definitions")]
    public class Line : BaseEntity
    {
        public string Name { get; set; }
        public int Number { get; set; }
        [JsonIgnore]
        public Factory? Factory { get; set; }
        public string? Type { get; set; }
        public int? FactoryId { get; set; }


        public int? ZoneId { get; set; }
        [Column(TypeName = "decimal(18,4)")]

        public decimal? RatioFromParent { get; set; }




        [JsonIgnore]
        public Zone? Zone { get; set; }
        [JsonIgnore]
        public ICollection<Machine>? Lines { get; set; }
        public string Area { get; set; }
        public string? ViewName { get; set; }
        public virtual ICollection<LineTransformer> LineTransformers { get; set; } = new HashSet<LineTransformer>();

    }
}
