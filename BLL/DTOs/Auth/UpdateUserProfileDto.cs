namespace BLL.DTOs.Auth
{
    public class UpdateUserProfileDto
    {
        //public string UserId { get; set; } = string.Empty;

        public string? Username { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        //public string? CurrentPassword { get; set; }

        //public string? NewPassword { get; set; }
    }
}
