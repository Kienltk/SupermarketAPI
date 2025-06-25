namespace SupermarketAPI.DTOs.Request
{
    public class RegisterDto
    {
        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = null!;
        public required string Country { get; set; }
        public required string Mobile { get; set; }
        public string Email { get; set; } = null!;
        public DateOnly Dob { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}