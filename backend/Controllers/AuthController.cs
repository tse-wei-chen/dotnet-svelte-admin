using backend.Common.Models;
using backend.Services.UserServices.Interface;
using backend.Services.UserServices.Request;
using backend.Services.UserServices.View;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpPost("login")]
        public ActionResult<ResponseBase<LoginVM>> Login([FromBody] LoginRequest request)
            => _authService.Login(request);

        [HttpPost("register")]
        public ActionResult<ResponseBase> Register([FromBody] RegisterRequest request)
            => _authService.Register(request);

        [HttpPost("refresh-token")]
        public ActionResult<ResponseBase<RefreshTokenVM>> RefreshToken([FromBody] RefreshTokenRequest request)
            => _authService.RefreshToken(request);
    }
}