using SupermarketAPI.DTOs.Request;
using SupermarketAPI.DTOs.Response;
using System.Security.Claims;

namespace SupermarketSystemAPI.Services
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterDto registerDto);
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<AuthResponseDto> RefreshTokenAsync(string refreshToken);
        Task LogoutAsync();
        Task UpdateUserInfoAsync(UpdateUserInfoDto updateDto);
        Task ChangePasswordAsync(string username, ChangePasswordDto dto);
        Task ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);
        Task VerifyCodeAsync(VerifyCodeDto dto);
        Task ResetPasswordAsync(ResetPasswordDto dto);
        Task<UserInfoResponseDto> GetUserInfoAsync(string username);
    }
}
