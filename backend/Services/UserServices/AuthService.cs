using backend.Common.Entities;
using backend.Common.Models;
using backend.Common.Utilites;
using backend.Common.Utilities;
using backend.Services.UserServices.Interface;
using backend.Services.UserServices.Model;
using backend.Services.UserServices.Request;
using backend.Services.UserServices.View;
using Microsoft.EntityFrameworkCore;

namespace backend.Services.UserServices
{
    public class AuthService(UserDbContext dbContext, IConfiguration configuration) : IAuthService
    {
        private readonly UserDbContext _dbContext = dbContext;
        private readonly IConfiguration _configuration = configuration;

        public async Task<ResponseBase> Register(RegisterInfo request)
        {
            await _dbContext.Users.AddAsync(new User
            {
                UserName = request.Name,
                Email = request.Email,
                PasswordHash = request.Password
            });
            await _dbContext.SaveChangesAsync();
            return ResponseBase.Ok("Registration successful.");
        }

        public async Task<ResponseBase<LoginVM>> Login(LoginInfo loginInfo)
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Email == loginInfo.Email);
            if (user == null || !Encrypt.Verify(loginInfo.Password, user.PasswordHash))
            {
                return ResponseBase<LoginVM>.Fail(ResponseStatusCode.Forbidden, "Invalid email or password.");
            }
            user.RefreshToken = JwtTokenHelper.GenerateRefreshToken();
            await _dbContext.SaveChangesAsync();

            return ResponseBase<LoginVM>.Ok(new LoginVM
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                AccessToken = JwtTokenHelper.GenerateToken(user.Id.ToString(), user.Email, _configuration),
                RefreshToken = user.RefreshToken
            }, "Login successful.");
        }

        public async Task<ResponseBase<RefreshTokenVM>> RefreshToken(RefreshTokenRequest request)
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.RefreshToken == request.RefreshToken);
            if (user == null)
            {
                return ResponseBase<RefreshTokenVM>.Fail(ResponseStatusCode.Forbidden, "Invalid refresh token.");
            }
            user.RefreshToken = JwtTokenHelper.GenerateRefreshToken();
            await _dbContext.SaveChangesAsync();

            return ResponseBase<RefreshTokenVM>.Ok(new RefreshTokenVM
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                AccessToken = JwtTokenHelper.GenerateToken(user.Id.ToString(), user.Email, _configuration),
                RefreshToken = user.RefreshToken
            }, "Token refreshed successfully.");
        }
    }
}