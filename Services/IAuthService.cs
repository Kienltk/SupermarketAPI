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
        Task ChangePasswordAsync(ClaimsPrincipal user, ChangePasswordDto dto);
        Task ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);
        Task VerifyCodeAsync(VerifyCodeDto dto);
        Task<UserInfoResponseDto> GetUserInfoAsync(ClaimsPrincipal user);
    }
}
