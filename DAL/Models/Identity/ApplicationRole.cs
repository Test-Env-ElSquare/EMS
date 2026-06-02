using Microsoft.AspNetCore.Identity;

namespace DAL.Models.Identity
{

    public class ApplicationRole : IdentityRole
    {
        public ICollection<ApplicationUserRole> UserRoles { get; set; } = new List<ApplicationUserRole>();

    }
}
