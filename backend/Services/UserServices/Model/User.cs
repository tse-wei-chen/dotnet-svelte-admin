namespace backend.Services.UserServices.Model
{
    public class User
    {
        public int Id { get; set; }

        public required string UserName { get; set; }

        public required string PasswordHash { get; set; }

        public required string Email { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiryTime { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
