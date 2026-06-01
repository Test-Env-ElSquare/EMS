namespace BLL.DTOs.Auth
{
    public class RegisterFactoryModel : RegisterModel
    {
        [Required(ErrorMessage = "Role name is required")]
        public string RoleName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        //public int? StationId { get; set; }
    }
    public class UserZoneDto
    {
        public int ZoneId { get; set; }
    }
}
