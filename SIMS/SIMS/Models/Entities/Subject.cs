using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIMS.Models.Entities
{
    public class Subject
    {
        [Key]
        public int SubjectId { get; set; }

        public int DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; } = null!;

        [Required, MaxLength(20)]
        public string SubjectCode { get; set; } = null!;

        [Required, MaxLength(150)]
        public string SubjectName { get; set; } = null!;

        public int Credits { get; set; }

        public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
