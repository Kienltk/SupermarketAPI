using System.ComponentModel.DataAnnotations;

namespace SupermarketAPI.DTOs.Request
{
    public class UpdateUserInfoDto
    {


        //[Required]
        [StringLength(50, MinimumLength = 1)]
        public string? FirstName { get; set; }

        [StringLength(50)]
        public string? MiddleName { get; set; }

        //[Required]
        [StringLength(50, MinimumLength = 1)]
        public string? LastName { get; set; }

        //[Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? Mobile { get; set; }

        [StringLength(100)]
        public string? Country { get; set; }
        [StringLength(100)]
        public string? HomePhone { get; set; }
        [StringLength(100)]
        public string? CreditCardNumber { get; set; }
        [StringLength(100)]
        public DateOnly? CreditCardExpiry { get; set; }
        public string? CardHolderName { get; set; }
        public string? CVV { get; set; }

        public DateOnly? Dob { get; set; }

        [StringLength(100)]
        public string? Street { get; set; }

        [StringLength(100)]
        public string? City { get; set; }

        [StringLength(100)]
        public string? State { get; set; }

        [StringLength(255)]
        public string? Address { get; set; }
    }
}
