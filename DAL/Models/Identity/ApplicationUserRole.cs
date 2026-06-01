using Microsoft.AspNetCore.Identity;

namespace DAL.Models.Identity
{

    public class ApplicationUserRole : IdentityUserRole<string>
    {
        public ApplicationUser User { get; set; } = null!;

        public ApplicationRole Role { get; set; } = null!;
    }
}

