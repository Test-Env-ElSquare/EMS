using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Identity
{

    [Table("SystemClaims", Schema = "Identity")]
    public class SystemClaim
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string ClaimName { get; set; }
    }
}
