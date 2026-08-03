using System.ComponentModel.DataAnnotations;

namespace SIMS.Models.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string Role { get; set; } = "Applicant";
        [Required, MaxLength(100)]
        public string FullName { get; set; } = null!;

        [Required, MaxLength(100)]
        public string Email { get; set; } = null!;

        [MaxLength(255)]
        public string? PasswordHash { get; set; }

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [MaxLength(255)]
        public string? Address { get; set; }

        public DateTime? DateOfBirth { get; set; }
        public string Status { get; set; } = "Active";

        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
    }
}