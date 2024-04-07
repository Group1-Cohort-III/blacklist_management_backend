using System.ComponentModel.DataAnnotations;

namespace BlackGuardApp.Application.DTOs.AuthenticationDTO
{
    public class SetPasswordDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and Confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
