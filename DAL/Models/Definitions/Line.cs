using DAL.Models;
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
        [JsonIgnore]
        public ICollection<Machine>? Lines { get; set; }
        public string Area { get; set; }
        public string? ViewName { get; set; }
        public virtual ICollection<LineTransformer> LineTransformers { get; set; } = new HashSet<LineTransformer>();

    }
}
