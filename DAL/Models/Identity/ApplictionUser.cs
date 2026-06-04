namespace DAL.Models.Identity
{

    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
        public bool ReceiveEmail { get; set; } = true;
        public ICollection<ApplicationUserRole> UserRoles { get; set; } = new List<ApplicationUserRole>();


    }

}
