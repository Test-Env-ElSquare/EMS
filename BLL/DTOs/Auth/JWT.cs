namespace BLL.DTOs.Auth
{
    public class JWT
    {
        public string Key { get; set; } = string.Empty;
        public string ValidIssuer { get; set; } = string.Empty;
        public string ValidAudience { get; set; } = string.Empty;
        public double DurationInDays { get; set; }
    }
}
