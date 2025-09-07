using backend.Common.Models;
using backend.Services.UserServices.Request;
using backend.Services.UserServices.View;

namespace backend.Services.UserServices.Interface
{
    public interface IAuthService
    {
        ResponseBase Register(RegisterInfo request);

        ResponseBase<LoginVM> Login(LoginInfo request);

        ResponseBase<RefreshTokenVM> RefreshToken(RefreshTokenRequest request);
    }
}