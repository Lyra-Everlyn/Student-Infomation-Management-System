using SIMS.Models.Entities;

namespace SIMS.Models.ViewModels
{
    public class ManageCourseViewModel
    {
        public Course Course { get; set; } = null!;
        public IEnumerable<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public IEnumerable<Assignment> Assignments { get; set; } = new List<Assignment>();
        public IEnumerable<Grade> Grades { get; set; } = new List<Grade>();
    }
}
