using System.ComponentModel.DataAnnotations;

namespace SupermarketAPI.DTOs.Request
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "New password is required.")]
        public string NewPassword { get; set; }
    }
}

   
