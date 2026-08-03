using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIMS.Models.Entities
{
    public class Grade
    {
        [Key]
        public int GradeId { get; set; }
        public int AssignmentId { get; set; }
        [ForeignKey("AssignmentId")]
        public virtual Assignment Assignment { get; set; } = null!;

        public int StudentId { get; set; }
        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; } = null!;

        [Column(TypeName = "decimal(5,2)")]
        public decimal? Score { get; set; }

        public DateTime? SubmitDate { get; set; }

        [MaxLength(255)]
        public string? FileUrl { get; set; } = null;
        public string? Feedback { get; set; } = null;
        public DateTime? GradedAt { get; set; } = null;
    }
}
