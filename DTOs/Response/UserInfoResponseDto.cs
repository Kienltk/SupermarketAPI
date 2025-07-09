namespace SupermarketAPI.DTOs.Response
{
    public class UserInfoResponseDto
    {
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? HomePhone { get; set; }
        public string? CreditCardNumber { get; set; }
        public DateOnly? CreditCardExpiry { get; set; }
        public string? CardHolderName { get; set; }
        public string? CVV { get; set; }
        public string? State { get; set; }
        public string? City {  get; set; }
        public string? Street { get; set; }
        public string? Mobile { get; set; }
        public string? Country { get; set; }
        public DateOnly? Dob { get; set; }
        public string? Address { get; set; }
    }
}
