using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIMS.Models.Entities
{
    public class Assignment
    {
        [Key]
        public int AssignmentId { get; set; }

        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; } = null!;

        [Required, MaxLength(100)]
        public string AssignmentName { get; set; } = null!;
        public string? Description { get; set; }

        public DateTime? StartTime { get; set; } = null;
        public DateTime? EndTime { get; set; } = null;

        public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
    }
}
