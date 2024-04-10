using System.ComponentModel.DataAnnotations;

namespace BlackGuardApp.Application.DTOs.AuthenticationDTO
{
    public class ValidateTokenDto
    {
        [Required(ErrorMessage = "Token is required")]
        public string Token { get; set; }
    }
}
