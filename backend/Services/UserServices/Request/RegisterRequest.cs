using System.ComponentModel.DataAnnotations;
using backend.Common.Utilities;

namespace backend.Services.UserServices.Request
{
    public class RegisterRequest
    {
        [Required]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }

        [Required]
        public required string Name { get; set; }
    }


    public class RegisterInfo
    {
        public required string Email { get; set; }

        public required string Password { get; set; }

        public required string Name { get; set; }

        public static implicit operator RegisterInfo(RegisterRequest src)
        {
            return new RegisterInfo
            {
                Email = src.Email,
                Password = Encrypt.Hash(src.Password),
                Name = src.Name
            };
        }
    }
}