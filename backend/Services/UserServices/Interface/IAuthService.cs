using backend.Common.Models;
using backend.Services.UserServices.Request;
using backend.Services.UserServices.View;

namespace backend.Services.UserServices.Interface
{
    public interface IAuthService
    {
        Task<ResponseBase> Register(RegisterInfo request);

        Task<ResponseBase<LoginVM>> Login(LoginInfo request);

        Task<ResponseBase<RefreshTokenVM>> RefreshToken(RefreshTokenRequest request);
    }
}