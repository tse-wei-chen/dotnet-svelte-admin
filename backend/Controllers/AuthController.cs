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
        public async Task<ActionResult<ResponseBase<LoginVM>>> Login([FromBody] LoginRequest request)
            => await _authService.Login(request);

        [HttpPost("register")]
        public async Task<ActionResult<ResponseBase>> Register([FromBody] RegisterRequest request)
            => await _authService.Register(request);

        [HttpPost("refresh-token")]
        public async Task<ActionResult<ResponseBase<RefreshTokenVM>>> RefreshToken([FromBody] RefreshTokenRequest request)
            => await _authService.RefreshToken(request);
    }
}