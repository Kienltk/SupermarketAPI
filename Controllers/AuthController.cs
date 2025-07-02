using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SupermarketAPI.DTOs.Request;
using SupermarketAPI.DTOs.Response;
using SupermarketSystemAPI.Services;

namespace SupermarketAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                await _authService.RegisterAsync(registerDto);
                var response = new ResponseObject<String>
                {
                    Code = 200,
                    Message = "Register successful",
                    Data = null
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseObject<AuthResponseDto>
                {
                    Code = 400,
                    Message = ex.Message,
                    Data = null
                };
                return BadRequest(errorResponse);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<ResponseObject<AuthResponseDto>>> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var response = await _authService.LoginAsync(loginDto);
                return Ok(new ResponseObject<AuthResponseDto>
                {
                    Code = 200,
                    Message = "Login successful",
                    Data = response

                });
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseObject<AuthResponseDto>
                {
                    Code = 400,
                    Message = ex.Message,
                    Data = null
                };
                return BadRequest(errorResponse);
            }
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<ResponseObject<AuthResponseDto>>> RefreshToken([FromBody] string refreshToken)
        {
            try
            {
                var response = await _authService.RefreshTokenAsync(refreshToken);
                return Ok(new ResponseObject<AuthResponseDto>
                {
                    Code = 200,
                    Message = "Refresh Token successful",
                    Data = response

                });
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseObject<AuthResponseDto>
                {
                    Code = 400,
                    Message = ex.Message,
                    Data = null
                };
                return BadRequest(errorResponse);
            }
        }
        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetUserInfo()
        {
            try
            {
                var result = await _authService.GetUserInfoAsync(User);
                return Ok(new ResponseObject<UserInfoResponseDto>
                {
                    Code = 200,
                    Message = "Lấy thông tin người dùng thành công",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseObject<string>
                {
                    Code = 400,
                    Message = ex.Message,
                    Data = null
                });
            }
        }
        [Authorize]
        [HttpPost("update-info")]
        public async Task<IActionResult> UpdateUserInfo([FromBody] UpdateUserInfoDto dto)
        {
            try
            {
                await _authService.UpdateUserInfoAsync(dto);
                return Ok(new ResponseObject<string>
                {
                    Code = 200,
                    Message = "Cập nhật thông tin thành công",
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseObject<string>
                {
                    Code = 400,
                    Message = ex.Message,
                    Data = null
                });
            }
        }
        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            try
            {
                await _authService.ChangePasswordAsync(User, dto);
                return Ok(new ResponseObject<string>
                {
                    Code = 200,
                    Message = "Đổi mật khẩu thành công",
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseObject<string>
                {
                    Code = 400,
                    Message = ex.Message,
                    Data = null
                });
            }
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            try
            {
                await _authService.ForgotPasswordAsync(dto);
                return Ok(new ResponseObject<string>
                {
                    Code = 200,
                    Message = "Mã xác nhận đã được gửi qua email",
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseObject<string>
                {
                    Code = 400,
                    Message = ex.Message,
                    Data = null
                });
            }
        }
        [HttpPost("verify-code")]
        public async Task<IActionResult> VerifyCode([FromBody] VerifyCodeDto dto)
        {
            try
            {
                await _authService.VerifyCodeAsync(dto);
                return Ok(new ResponseObject<string>
                {
                    Code = 200,
                    Message = "Xác minh mã thành công",
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseObject<string>
                {
                    Code = 400,
                    Message = ex.Message,
                    Data = null
                });
            }
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _authService.LogoutAsync();
                var response = new ResponseObject<String>
                {
                    Code = 200,
                    Message = "Logout successful",
                    Data = null
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
