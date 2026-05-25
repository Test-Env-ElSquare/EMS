using DAL.Models;
using DAL.Models.Definitions;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Definitions
{
    [Table(name: "LineTransformers", Schema = "Definitions")]

    public class LineTransformer : BaseEntity
    {
        public int LineId { get; set; }
        public Line Line { get; set; }
        public int TransformerId { get; set; }
        public Transformer Transformer { get; set; }

    }
}
