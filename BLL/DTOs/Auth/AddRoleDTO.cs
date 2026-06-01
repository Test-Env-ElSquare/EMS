

namespace BLL.DTOs.Auth
{
    public class AddRoleDTO
    {
        public string RoleName { get; set; }
        public List<ClaimDto> Claims { get; set; }
    }
}
