using System.ComponentModel.DataAnnotations;

namespace StudyBuddy.Core.DTOs
{
    public class LoginDto
    {
        [Required]
        public string Identifier { get; set; } = null!; 

        [Required]
        public string Password { get; set; } = null!;
    }
}
