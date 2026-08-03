using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIMS.Models.Entities
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }

        public int SubjectId { get; set; }
        [ForeignKey("SubjectId")]
        public virtual Subject Subject { get; set; } = null!;

        public int? FacultyId { get; set; }
        [ForeignKey("FacultyId")]
        public virtual Faculty? Faculty { get; set; }

        [Required, MaxLength(150)]
        public string CourseName { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public virtual ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
    }
}
