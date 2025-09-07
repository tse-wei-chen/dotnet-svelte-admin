using System.ComponentModel.DataAnnotations;

namespace backend.Services.UserServices.Request
{
    public class LoginRequest
    {
        [Required]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }
    }

    public class LoginInfo
    {
        public required string Email { get; set; }

        public required string Password { get; set; }

        public static implicit operator LoginInfo(LoginRequest src)
        {
            return new LoginInfo
            {
                Email = src.Email,
                Password = src.Password
            };
        }
    }
}