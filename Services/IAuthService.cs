using SupermarketAPI.DTOs.Request;
using SupermarketAPI.DTOs.Response;

namespace SupermarketSystemAPI.Services
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterDto registerDto);
        Task<ResponseObject<AuthResponseDto>> LoginAsync(LoginDto loginDto);
        Task<AuthResponseDto> RefreshTokenAsync(string refreshToken);
    }
}
