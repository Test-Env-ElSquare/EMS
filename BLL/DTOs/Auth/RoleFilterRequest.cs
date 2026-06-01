namespace BLL.DTOs.Auth
{
    public class RoleFilterRequest
    {
        public string? RoleName { get; set; }
        public List<string>? ClaimValues { get; set; }
    }
}
