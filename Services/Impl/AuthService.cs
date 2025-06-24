using Microsoft.IdentityModel.Tokens;
using SupermarketAPI.DTOs.Request;
using SupermarketAPI.DTOs.Response;
using SupermarketAPI.Models;
using SupermarketAPI.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SupermarketSystemAPI.Services;

namespace SupermarketAPI.Services.Impl
{
    public class AuthService : IAuthService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IConfiguration _configuration;
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public AuthService(ICustomerRepository customerRepository, IConfiguration configuration)
        {
            _customerRepository = customerRepository;
            _configuration = configuration;
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        public async Task RegisterAsync(RegisterDto registerDto)
        {
            var existingCustomer = await _customerRepository.GetCustomerByUsernameAsync(registerDto.Username);
            if (existingCustomer != null)
                throw new Exception("Username already exists");

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            var customer = new Customer
            {
                FirstName = registerDto.FirstName,
                MiddleName = registerDto.MiddleName,
                LastName = registerDto.LastName,
                Street = registerDto.Street,
                City = registerDto.City,
                State = registerDto.State,
                Country = registerDto.Country,
                HomePhone = registerDto.HomePhone,
                Mobile = registerDto.Mobile,
                CreditCardNumber = null,
                CreditCardExpiry = null,
                Email = registerDto.Email,
                Dob = registerDto.Dob,
                Username = registerDto.Username,
                Password = hashedPassword,
                Role = "USER"
            };

            await _customerRepository.AddCustomerAsync(customer);
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            var customer = await _customerRepository.GetCustomerByUsernameAsync(loginDto.Username);
            if (customer == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, customer.Password))
                throw new Exception("Invalid username or password");

            var accessToken = GenerateAccessToken(customer);
            var refreshToken = GenerateRefreshToken(customer);

            AuthResponseDto authResponseDto = new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            return authResponseDto;

        }

        public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken)
        {
            try
            {
                var principal = GetPrincipalFromToken(refreshToken);
                var username = principal.Identity.Name;
                var customer = await _customerRepository.GetCustomerByUsernameAsync(username);

                if (customer == null)
                    throw new Exception("Invalid refresh token");

                var newAccessToken = GenerateAccessToken(customer);
                var newRefreshToken = GenerateRefreshToken(customer);

                return new AuthResponseDto
                {
                    AccessToken = newAccessToken
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid refresh token", ex);
            }
        }

        public async Task LogoutAsync()
        {
            await Task.CompletedTask;
        }

        private string GenerateAccessToken(Customer customer)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, customer.CustomerId.ToString()),
                new Claim(ClaimTypes.Name, customer.Username),
                new Claim(ClaimTypes.Role, customer.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: creds
            );

            return _tokenHandler.WriteToken(token);
        }

        private string GenerateRefreshToken(Customer customer)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, customer.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: creds
            );

            return _tokenHandler.WriteToken(token);
        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]))
            };

            var principal = _tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
            if (!(validatedToken is JwtSecurityToken jwtToken) || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    }
}