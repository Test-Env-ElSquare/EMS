namespace BLL.DTOs.Auth
{
    public class UpdateRoleDto
    {
        public string RoleName { get; set; }
        public List<ClaimDto>? Claims { get; set; }
        public List<int>? zones { get; set; }
        // 👈 جديد: Areas
        public List<int>? AreaIds { get; set; }

    }
}
