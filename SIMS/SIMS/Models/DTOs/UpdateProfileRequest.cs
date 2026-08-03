using System.ComponentModel.DataAnnotations;

namespace SIMS.Models.DTOs
{
    public class UpdateProfileRequest
    {
        [Required, MaxLength(100)]
        public string FullName { get; set; } = null!;

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [MaxLength(255)]
        public string? Address { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [Required]
        public int DepartmentId { get; set; }
    }
}
