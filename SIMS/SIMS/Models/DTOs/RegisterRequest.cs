using System.ComponentModel.DataAnnotations;

namespace SIMS.Models.DTOs
{
    public class RegisterRequest
    {
        [Required, MaxLength(100)]
        public string FullName { get; set; } = null!;

        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; } = null!;

        [Required, MaxLength(20)]
        public string PhoneNumber { get; set; } = null!;

        [Required, MinLength(6)]
        public string Password { get; set; } = null!;
    }
}
