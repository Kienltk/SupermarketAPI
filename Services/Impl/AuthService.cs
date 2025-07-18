using Microsoft.IdentityModel.Tokens;
using SupermarketAPI.DTOs.Request;
using SupermarketAPI.DTOs.Response;
using SupermarketAPI.Models;
using SupermarketAPI.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SupermarketSystemAPI.Services;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;
using SupermarketAPI.Data;

namespace SupermarketAPI.Services.Impl
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private readonly IMemoryCache _cache;
        private readonly ICustomerRepository _customerRepository;
        private readonly IConfiguration _configuration;
        private readonly SupermarketContext _context;
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public AuthService(IMemoryCache cache, SupermarketContext context, IConfiguration config, ICustomerRepository customerRepository, IConfiguration configuration)
        {
            _context = context;
            _cache = cache;
            _config = config;
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
                Street = null,
                City = null,
                State = null,
                Country = registerDto.Country,
                HomePhone = null,
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
            string fullName = string.Join(" ", new[] { customer.FirstName, customer.MiddleName, customer.LastName }
                                     .Where(s => !string.IsNullOrWhiteSpace(s)));
            var claims = new[]
            {
                new Claim("id", customer.CustomerId.ToString()),
                new Claim("sub", customer.Username),
                new Claim("fullName", fullName),
                new Claim("role", customer.Role)
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
                new Claim("sub", customer.Username)
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

        public async Task ChangePasswordAsync(string username, ChangePasswordDto dto)
        {
            var customer = await _customerRepository.GetCustomerByUsernameAsync(username);
            if (customer == null)
                throw new Exception("User not found");

            if (!BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, customer.Password))
                throw new Exception("Mật khẩu hiện tại không chính xác");

            customer.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            await _customerRepository.UpdateCustomerAsync(customer);
        }

        public async Task ForgotPasswordAsync(ForgotPasswordDto dto)
        {
            var customer = await _customerRepository.GetCustomerByEmailAsync(dto.Email);
            if (customer == null)
                throw new Exception("Không tìm thấy người dùng với email này.");

            string verificationCode = Generate6DigitCode();

            _cache.Set(dto.Email, verificationCode, TimeSpan.FromMinutes(10));

            await SendEmailAsync(dto.Email, verificationCode);
        }

        private string Generate6DigitCode()
        {
            Random rnd = new Random();
            return rnd.Next(100000, 999999).ToString();
        }

        public Task VerifyCodeAsync(VerifyCodeDto dto)
        {
            if (_cache.TryGetValue(dto.Email, out string cachedCode))
            {
                if (cachedCode == dto.Code)
                {
                    _cache.Remove(dto.Email);
                    _cache.Set(dto.Email + "_verified", DateTime.UtcNow, TimeSpan.FromMinutes(15));
                    return Task.CompletedTask;
                }
            }

            throw new Exception("Mã xác nhận không đúng hoặc đã hết hạn.");
        }


        private async Task SendEmailAsync(string toEmail, string code)
        {
            var fromAddress = new MailAddress("Ndc7571@gmail.com", "Supermarket Support");
            var toAddress = new MailAddress(toEmail);
            string fromPassword = "rejdvgxpaheddjgt";
            string subject = "Xác nhận đặt lại mật khẩu";
            string body = $"Mã xác nhận của bạn là: {code}. Mã có hiệu lực trong 10 phút.";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            };
            await smtp.SendMailAsync(message);
        }

        public async Task<UserInfoResponseDto> GetUserInfoAsync(string username)
        {
            if (string.IsNullOrEmpty(username))
                throw new Exception("Invalid token");

            var customer = await _customerRepository.GetCustomerByUsernameAsync(username);
            if (customer == null)
                throw new Exception("User not found");

            return new UserInfoResponseDto
            {
                Email = customer.Email,
                FirstName = customer.FirstName,
                MiddleName = customer.MiddleName ?? "",
                LastName = customer.LastName,
                HomePhone = customer.HomePhone,
                CreditCardNumber = customer.CreditCardNumber,
                CreditCardExpiry = customer.CreditCardExpiry,
                CardHolderName = customer.CardHolderName,
                CVV = customer.CVV,
                State = customer.State,
                City = customer.City,
                Street = customer.Street,
                Mobile = customer.Mobile,
                Country = customer.Country,
                Dob = customer.Dob
            };
        }

        public async Task<UserInfoResponseDto> UpdateUserInfoAsync(string username, UpdateUserInfoDto updateDto)
        {
            try
            {
                var customer = await _customerRepository.GetCustomerByUsernameAsync(username);
                if (customer == null)
                    throw new Exception("User not found");

                customer.FirstName = updateDto.FirstName ?? customer.FirstName;
                customer.LastName = updateDto.LastName ?? customer.LastName;
                customer.Email = updateDto.Email ?? customer.Email;


                customer.MiddleName = updateDto.MiddleName ?? customer.MiddleName;
                customer.Mobile = updateDto.Mobile ?? customer.Mobile;
                customer.Country = updateDto.Country ?? customer.Country;
                customer.HomePhone = updateDto.HomePhone ?? customer.HomePhone;
                customer.CreditCardNumber = updateDto.CreditCardNumber ?? customer.CreditCardNumber;
                customer.CreditCardExpiry = updateDto.CreditCardExpiry ?? customer.CreditCardExpiry;
                customer.CardHolderName = updateDto.CardHolderName ?? customer.CardHolderName;
                customer.CVV = updateDto.CVV ?? customer.CVV;
                customer.Dob = updateDto.Dob ?? customer.Dob;
                customer.Street = updateDto.Street ?? customer.Street;
                customer.City = updateDto.City ?? customer.City;
                customer.State = updateDto.State ?? customer.State;

                await _customerRepository.UpdateCustomerAsync(customer);

                return new UserInfoResponseDto
                {
                    Email = customer.Email,
                    FirstName = customer.FirstName,
                    MiddleName = customer.MiddleName ?? "",
                    LastName = customer.LastName,
                    HomePhone = customer.HomePhone,
                    CreditCardNumber = customer.CreditCardNumber,
                    CreditCardExpiry = customer.CreditCardExpiry,
                    CardHolderName = customer.CardHolderName,
                    CVV = customer.CVV,
                    State = customer.State,
                    City = customer.City,
                    Street = customer.Street,
                    Mobile = customer.Mobile,
                    Country = customer.Country,
                    Dob = customer.Dob
                };
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Save error: " + ex.InnerException?.Message);
                throw; 
            }
        }


        public async Task ResetPasswordAsync(ResetPasswordDto dto)
        {
            if (!_cache.TryGetValue(dto.Email + "_verified", out DateTime verifiedTime) ||
                DateTime.UtcNow - verifiedTime > TimeSpan.FromMinutes(15))
            {
                throw new Exception("Email chưa được xác minh hoặc mã đã hết hạn.");
            }

            var customer = await _customerRepository.GetCustomerByEmailAsync(dto.Email);
            if (customer == null)
                throw new Exception("Không tìm thấy người dùng.");

            customer.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            await _customerRepository.UpdateCustomerAsync(customer);

            _cache.Remove(dto.Email);
            _cache.Remove(dto.Email + "_verified");
        }

        public async Task<List<Customer>> GetAllUsersAsync()
        {
            return await _context.Customers.ToListAsync();
        }
    }
}
