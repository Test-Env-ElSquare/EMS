using DAL.Models;
using DAL.Models.Definitions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace Domain.Models.Definitions
{
    [Table(name: "Factories", Schema = "Definitions")]
    public class Factory : BaseEntity
    {
        public string Name { get; set; }
        public string Location { get; set; }
        [JsonIgnore]
        public ICollection<Line>? Lines { get; set; }

        [JsonIgnore]
        public ICollection<Zone>? Zones { get; set; }
    }
}
