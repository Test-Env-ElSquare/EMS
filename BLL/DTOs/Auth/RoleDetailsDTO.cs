namespace BLL.DTOs.Auth
{
    public class RoleDetailsDTO
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public List<ClaimDto> Claims { get; set; } = new();
        public List<ZoneDTO> Areas { get; set; } = new();
    }
}
