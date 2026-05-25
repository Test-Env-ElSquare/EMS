using Domain.Models.Definitions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DAL.Models.Definitions
{
    [Table(name: "Transformer", Schema = "Definitions")]
    public class Transformer : BaseEntity
    {
        public string TransformerName { set; get; }

        public virtual ICollection<LineTransformer> LineTransformers { get; set; } = new HashSet<LineTransformer>();
        public int? FactoryId { set; get; }
        [JsonIgnore]
        public Factory? factory { get; set; }
    }
}
