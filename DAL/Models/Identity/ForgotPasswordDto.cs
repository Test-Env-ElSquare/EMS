namespace DAL.Models.Identity
{
    public class PasswordResetOtp
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Code { get; set; }
        public DateTime ExpirationTime { get; set; }
        public DateTime CreatedAt { get; set; }

        public int RequestCount { get; set; } = 1;
        public DateTime LastAttempt { get; set; } = DateTime.UtcNow;
        public DateTime? LockedUntil { get; set; }
        public bool IsVerified { get; set; } = false;

    }
}
