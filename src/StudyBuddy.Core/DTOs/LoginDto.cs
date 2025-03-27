using System.ComponentModel.DataAnnotations;

namespace StudyBuddy.Core.DTOs
{
    public class LoginDto
    {
        [Required]
        public string Identifier { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; } = string.Empty;
    }
}
