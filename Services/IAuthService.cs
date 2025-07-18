using SupermarketAPI.DTOs.Request;
using SupermarketAPI.DTOs.Response;
using SupermarketAPI.Models;
using System.Security.Claims;

namespace SupermarketSystemAPI.Services
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterDto registerDto);
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<AuthResponseDto> RefreshTokenAsync(string refreshToken);
        Task LogoutAsync();
        Task<UserInfoResponseDto> UpdateUserInfoAsync(string username, UpdateUserInfoDto updateDto);
        Task ChangePasswordAsync(string username, ChangePasswordDto dto);
        Task ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);
        Task VerifyCodeAsync(VerifyCodeDto dto);
        Task<UserInfoResponseDto> GetUserInfoAsync(string username);
        Task ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
        Task<List<Customer>> GetAllUsersAsync();
    }
}
