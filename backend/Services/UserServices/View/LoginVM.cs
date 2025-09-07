using backend.Services.UserServices.Model;

namespace backend.Services.UserServices.View
{
    public class LoginVM
    {
        public int Id { get; set; }

        public string? UserName { get; set; }

        public string? Email { get; set; }

        public string? AccessToken { get; set; }

        public string? RefreshToken { get; set; }

        public static implicit operator LoginVM(User src)
        {
            return new LoginVM
            {
                Id = src.Id,
                UserName = src.UserName,
                Email = src.Email,
            };
        }
    }
}