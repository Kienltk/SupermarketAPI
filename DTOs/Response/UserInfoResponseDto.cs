namespace SupermarketAPI.DTOs.Response
{
    public class UserInfoResponseDto
    {
        public int CustomerId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? Mobile { get; set; }
        public string? Country { get; set; }
        public DateOnly? Dob { get; set; }
    }
}
