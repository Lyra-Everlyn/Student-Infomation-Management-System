using System.ComponentModel.DataAnnotations;

namespace SIMS.Models.Entities
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }
        [Required, MaxLength(20)]
        public string DepartmentCode { get; set; } = null!;

        [Required, MaxLength(100)]
        public string DepartmentName { get; set; } = null!;

        public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>();
        public virtual ICollection<Faculty> Faculties { get; set; } = new List<Faculty>();
    }
}
